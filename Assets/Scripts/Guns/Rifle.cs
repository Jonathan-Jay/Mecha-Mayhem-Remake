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

	public int Shoot(Vector3 start, Vector3 direction, Vector3 muzzel) {
		int killed = 0;
		//TODO: SHOOT
		if (cooldown <= 0) {
			--ammo;
			RaycastHit hit;
			Vector3 endPos = start + direction * range;
			if (Physics.Raycast(start, direction, out hit, range)) {
				if (hit.transform.CompareTag("Turret")) {
					if (hit.transform.GetComponent<TrackingFiring>().TakeDamage(damage)) {
						++killed;
					}
					++killed;
				}
				endPos = hit.point;
			}
			LazerBeam.CreateBeam(laserPrefab, muzzel, endPos, 0.1f);
			cooldown = 1.5f;
		}
		return killed;
	}

	public bool Reload() {
		if (ammo == 10)
			return false;
		ammo = 10;
		return true;
	}

	public bool GetAuto() {
		return false;
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
		return Gun.GunType.Rifle;
	}
}
