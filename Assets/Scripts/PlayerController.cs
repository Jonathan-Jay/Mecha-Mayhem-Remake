using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public CharController controller;
	public HUDManager hud;
	public Transform hand;

    private float health;
    private float dash;
	private Gun offhand;
	private Gun mainhand;

	void Start()
	{
		health = hud.healthMax;
		dash = hud.dashMax;

		hud.SetScore(0);

		offhand = new Pistol();
		mainhand = new Rifle();

		UpdateHUD();
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.G)) {
			hud.SetScore(Random.Range(0, 100));
		}

		if (dash < hud.dashMax) {
			dash += Time.deltaTime;
			if (dash >= hud.dashMax)
				dash = hud.dashMax;
			hud.SetDash(dash);
		}
		else if (Input.GetButton("Dash")) {
			controller.DoDash();
			dash = 0;
			hud.SetDash(dash);
		}

        if (Input.GetButtonDown("SwapWeapon")) {
			if (offhand != null) {
				SwapWeapon();
			}
		}

		if (mainhand != null) {
			mainhand.Update();
			if (Input.GetButton("Fire")) {
				mainhand.Shoot(hand.position, transform.forward);
				if (mainhand.GetAmmoPercent() <= 0) {
					hud.SetMainWeapon(IconStorage.gunIcons[(int)Gun.GunType.Empty]);
					mainhand = null;
					SwapWeapon();
				}
				else hud.SetAmmoBar(mainhand.GetAmmoPercent());
			}
		}
    }

	void SwapWeapon() {
		Gun temp = offhand;
		offhand = mainhand;
		mainhand = temp;
		hud.SwapWeapons();
		if (mainhand != null) {
			mainhand.SetCooldown(2f);
			hud.SetAmmoBar(mainhand.GetAmmoPercent());
		}
		else	hud.SetAmmoBar(0);
	}

	void UpdateHUD() {
		hud.SetHealth(health);
		hud.SetDash(dash);
		hud.SetOffhandWeapon(IconStorage.gunIcons[(int)offhand.GetGunType()]);
		hud.SetMainWeapon(IconStorage.gunIcons[(int)mainhand.GetGunType()]);
		if (mainhand == null)	hud.SetAmmoBar(0f);
		else	hud.SetAmmoBar(mainhand.GetAmmoPercent());
	}
}
