using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1
public class Pistol : Gun
{
	static float damage = 15f;
	static float range = 50f;
	static float div = 1f / 30f;
	public static GameObject laserPrefab;

	int ammo = 30;
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
			cooldown = 0.5f;
		}
		return killed;
	}

	public bool Reload() {
		if (ammo == 30)
			return false;
		ammo = 30;
		return true;
	}

	public bool GetAuto() {
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
