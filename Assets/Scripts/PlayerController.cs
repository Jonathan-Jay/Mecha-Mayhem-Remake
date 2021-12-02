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
	private int hasHeal = 0;
	private int score = 0;

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

		if (!hud.mobileMode) {
        	if (Input.GetButtonDown("SwapWeapon")) {
				if (offhand != null) {
					SwapWeapon();
				}
			}

        	if (hud.DropWeaponHold(Input.GetButton("Drop"))) {
				DropWeapon();
			}

			if (Input.GetButtonDown("Heal")) {
				if (hasHeal > 0 && health != hud.healthMax) {
					hud.SetHealthPickup(--hasHeal);
					health = Mathf.Min(health + 25, hud.healthMax);
					hud.SetHealth(health);
				}
			}

			if (mainhand != null) {
				mainhand.Update();
				if (Input.GetButtonDown("Fire") || (mainhand.GetAuto() && Input.GetButton("Fire"))) {
					int change = mainhand.Shoot(hud.transform.position, hud.transform.forward, gun.GetChild(0).position);
					if (change == 2) {
						++score;
						UpdateHUD();
					}

					if (mainhand.GetAmmoPercent() <= 0) {
						DropWeapon();
					}
					else hud.SetAmmoBar(mainhand.GetAmmoPercent());
				}
			}
		}
		else {
			if (hud.GetSwapWeaponInput()) {
				if (offhand != null) {
					SwapWeapon();
				}
			}

			if (hud.GetDropWeaponInput()) {
				DropWeapon();
			}

			if (hud.GetHealInput()) {
				if (hasHeal > 0 && health != hud.healthMax) {
					hud.SetHealthPickup(--hasHeal);
					health = Mathf.Min(health + 25, hud.healthMax);
					hud.SetHealth(health);
				}
			}

			if (mainhand != null) {
				mainhand.Update();
				if (hud.GetShootInput(mainhand.GetAuto())) {
					int change = mainhand.Shoot(hud.transform.position, hud.transform.forward, gun.GetChild(0).position);
					if (change == 2) {
						++score;
						UpdateHUD();
					}

					if (mainhand.GetAmmoPercent() <= 0) {
						DropWeapon();
					}
					else hud.SetAmmoBar(mainhand.GetAmmoPercent());
				}
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
		hud.SetScore(score);
		if (offhand != null)
			hud.SetOffhandWeapon(IconStorage.gunIcons[(int)offhand.GetGunType()]);
		if (mainhand != null)
			hud.SetMainWeapon(IconStorage.gunIcons[(int)mainhand.GetGunType()]);
		
		if (mainhand == null)	hud.SetAmmoBar(0f);
		else	hud.SetAmmoBar(mainhand.GetAmmoPercent());
		hud.SetHealthPickup(hasHeal);
	}

	void DropWeapon() {
		if (mainhand != null) {
			hud.SetMainWeapon(IconStorage.gunIcons[(int)Gun.GunType.Empty]);
			mainhand = null;
			Destroy(gun.gameObject);
			SwapWeapon();
		}
	}

	public bool PickUpHealPack() {
		if (hasHeal == 2)
			return false;
		hasHeal = 2;
		UpdateHUD();
		return true;
	}

	public bool TakeDamage(float dmg) {
		health = Mathf.Max(health - dmg, 0f);
		hud.SetHealth(health);
		if (health <= 0) {
			//dies
			return true;
		}
		return false;
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
			if (mainhand.Reload()) {
				hud.SetAmmoBar(mainhand.GetAmmoPercent());
				return true;
			}
			else	return false;
		}
		//then if offhand is empty
		else if (offhand == null) {
			offhand = IconStorage.GetGunFromType(weapon);
			UpdateHUD();
			return true;
		}
		//then if it matches
		else if (offhand.GetGunType() == weapon) {
			return offhand.Reload();
		}
		return false;
	}

	public bool dead() {
		return health <= 0f;
	}
}