using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2
public class Rifle : Gun
{
	static float damage = 50f;
	static float range = 100f;
	static float div = 1f / 10f;
	public static GameObject laserPrefab;

	int ammo = 10;
	float cooldown = 0;

	public override int Shoot(Vector3 start, Vector3 direction, Vector3 muzzel, AudioSource pew) {
		int killed = 0;
		//TODO: SHOOT
		if (cooldown <= 0) {
			--ammo;
			Vector3 endPos;
			killed = ShootLaser(start, direction, out endPos, range, damage);
			pew.Play();
			LazerBeam.CreateBeam(laserPrefab, muzzel, endPos, 0.1f);
			cooldown = 1.5f;
		}
		return killed;
	}

	public override bool Reload() {
		if (ammo == 10)
			return false;
		ammo = 10;
		return true;
	}

	public override bool GetAuto() {
		return false;
	}

	public override void Update() {
		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		}
	}

	public override void SetCooldown(float amt) {
		cooldown = amt;
	}

	public override float GetAmmoPercent() {
		return ((float)ammo) * div;
	}

	public override Gun.GunType GetGunType() {
		return Gun.GunType.Rifle;
	}
}
