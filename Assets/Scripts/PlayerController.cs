using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public AudioSource pew;
	public AudioSource drop;
	public CharController controller;
	public CameraController cam;
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

		UpdateHUD();
		hud.CanShoot(mainhand != null);
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.G)) {
			hud.SetScore(Random.Range(0, 100));
		}

		if (hud.mobileMode) {
			if (dash < hud.dashMax) {
				dash += Time.deltaTime;
				if (dash >= hud.dashMax)
					dash = hud.dashMax;
				hud.SetDash(dash);
			}
			else if (hud.dashButton.touched) {
				controller.DoDash();
				dash = 0;
				hud.SetDash(dash);
			}

			if (hud.GetZoomInput()) {
				if (cam.distance != -1f) {
					cam.distance = -1f;
					cam.orginOffsetWithRotation.x = 0.5f;
					cam.sensitivity = 0.9f;
				}
			}
			else if (cam.distance != 3f) {
				cam.distance = 3f;
				cam.orginOffsetWithRotation.x = 1f;
				cam.sensitivity = 1.7f;
			}

			if (hud.GetDropWeaponInput()) {
				DropWeapon();
			}
			if (hud.weaponButton.GetUp()) {
				if (offhand != null) {
					SwapWeapon();
				}
			}

			if (hud.GetHealInput()) {
				if (hasHeal > 0 && health != hud.healthMax) {
					hud.SetHealthPickup(--hasHeal);
					health = Mathf.Min(health + 25, hud.healthMax);
					pew.Play();
					hud.SetHealth(health);
				}
			}

			if (mainhand != null) {
				mainhand.Update();
				if (hud.GetShootInput(mainhand.GetAuto())) {
					int change = mainhand.Shoot(hud.transform.position, hud.transform.forward, gun.GetChild(0).position, pew);
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

			if (Input.GetButton("Zoom")) {
				if (cam.distance != -1f) {
					cam.distance = -1f;
					cam.orginOffsetWithRotation.x = 0.5f;
					cam.sensitivity = 0.9f;
				}
			}
			else if (cam.distance != 3f) {
				cam.distance = 3f;
				cam.orginOffsetWithRotation.x = 1f;
				cam.sensitivity = 1.7f;
			}

        	if (hud.DropWeaponHold(Input.GetButton("Drop"))) {
				DropWeapon();
			}
			if (Input.GetButtonDown("SwapWeapon")) {
				if (offhand != null) {
					SwapWeapon();
				}
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
					int change = mainhand.Shoot(hud.transform.position, hud.transform.forward, gun.GetChild(0).position, pew);
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
			drop.Play();
			Destroy(gun.gameObject);
			SwapWeapon();
		}
		hud.CanShoot(mainhand != null);
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

	//returns 1 for gun, 2 for reload
	public int PickUpWeapon(Gun.GunType weapon) {
		//first check if main hand is empty
		if (mainhand == null) {
			//if main is empty, you can give gun right away
			mainhand = IconStorage.GetGunFromType(weapon);
			if (weapon != Gun.GunType.Empty) {
				gun = Instantiate(IconStorage.gunPrefabs[(int)weapon], hand).transform;
			}
			hud.CanShoot(true);
			UpdateHUD();
			return 1;
		}
		//then if it matches
		else if (mainhand.GetGunType() == weapon) {
			if (mainhand.Reload()) {
				hud.SetAmmoBar(mainhand.GetAmmoPercent());
				return 2;
			}
		}
		//then if offhand is empty
		else if (offhand == null) {
			offhand = IconStorage.GetGunFromType(weapon);
			UpdateHUD();
			return 1;
		}
		//then if it matches
		else if (offhand.GetGunType() == weapon) {
			if (offhand.Reload())
				return 2;
		}
		return 0;
	}

	public bool dead() {
		return health <= 0f;
	}
}
