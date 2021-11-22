using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
	static float damage = 15f;
	static float range = 50f;
	static float div = 1f / 30f;
	public static GameObject laserPrefab;

	int ammo = 30;
	float cooldown = 0;

    public bool Shoot(Vector3 start, Vector3 direction) {
		//TODO: SHOOT
		if (cooldown <= 0) {
			--ammo;
			LazerBeam.CreateBeam(laserPrefab, start, start + direction * range, 0.1f);
			cooldown = 0.1f;
		}
		return false;
	}

	public bool Reload() {
		if (ammo == 30)
			return false;
		ammo = 30;
		return true;
	}

    public void Update() {
        if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		}
    }

	public void SetCooldown(float amt) {
		cooldown = amt;
	}

	public float GetAmmoPercent() {
		return ((float)ammo) * div;
	}

	public Gun.GunType GetGunType() {
		return Gun.GunType.Pistol;
	}
}
