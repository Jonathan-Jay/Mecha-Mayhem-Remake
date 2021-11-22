using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public CharController controller;
	public HUDManager hud;
	public Transform hand;
	Transform gun = null;

    private float health;
    private float dash;
	private Gun offhand;
	private Gun mainhand;

	void Start()
	{
		health = hud.healthMax;
		dash = hud.dashMax;

		hud.SetScore(0);

		PickUpWeapon(Gun.GunType.Pistol);
		PickUpWeapon(Gun.GunType.Rifle);

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
				mainhand.Shoot(gun.GetChild(0).position, hud.transform.forward);
				if (mainhand.GetAmmoPercent() <= 0) {
					DropWeapon();
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
			if (gun != null) {
				Destroy(gun.gameObject);
			}
			gun = Instantiate(IconStorage.gunPrefabs[(int)mainhand.GetGunType()], hand).transform;

			mainhand.SetCooldown(2f);
			hud.SetAmmoBar(mainhand.GetAmmoPercent());
		}
		else	hud.SetAmmoBar(0);
	}

	void UpdateHUD() {
		hud.SetHealth(health);
		hud.SetDash(dash);
		if (offhand != null)
			hud.SetOffhandWeapon(IconStorage.gunIcons[(int)offhand.GetGunType()]);
		if (mainhand != null)
			hud.SetMainWeapon(IconStorage.gunIcons[(int)mainhand.GetGunType()]);
		
		if (mainhand == null)	hud.SetAmmoBar(0f);
		else	hud.SetAmmoBar(mainhand.GetAmmoPercent());
	}

	void DropWeapon() {
		if (mainhand != null) {
			hud.SetMainWeapon(IconStorage.gunIcons[(int)Gun.GunType.Empty]);
			mainhand = null;
			Destroy(gun.gameObject);
			SwapWeapon();
		}
	}

	public bool PickUpWeapon(Gun.GunType weapon) {
		//first check if main hand is empty
		if (mainhand == null) {
			//if main is empty, you can give gun right away
			mainhand = IconStorage.GetGunFromType(weapon);
			if (weapon != Gun.GunType.Empty) {
				gun = Instantiate(IconStorage.gunPrefabs[(int)weapon], hand).transform;
			}
			UpdateHUD();
			return true;
		}
		//then if it matches
		else if (mainhand.GetGunType() == weapon) {
			mainhand.Reload();
			return true;
		}
		//then if offhand is empty
		else if (offhand == null) {
			offhand = IconStorage.GetGunFromType(weapon);
			UpdateHUD();
			return true;
		}
		//then if it matches
		else if (offhand.GetGunType() == weapon) {
			offhand.Reload();
			return true;
		}
		return false;
	}
}
