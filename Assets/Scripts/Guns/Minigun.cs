using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3
public class Minigun : Gun
{
	static float damage = 10f;
	static float range = 50f;
	static float div = 1f / 50f;
	public static GameObject laserPrefab;

	int ammo = 50;
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
			cooldown = 0.1f;
		}
		return killed;
	}

	public override bool Reload() {
		if (ammo == 50)
			return false;
		ammo = 50;
		return true;
	}

	public override bool GetAuto() {
		return true;
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
		return Gun.GunType.MachineGun;
	}
}
