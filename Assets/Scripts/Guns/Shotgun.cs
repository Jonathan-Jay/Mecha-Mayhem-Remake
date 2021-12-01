using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//4
public class Shotgun : Gun
{
	static float damage = 50f;
	static float range = 100f;
	static float div = 1f / 10f;
	public static GameObject laserPrefab;

	int ammo = 10;
	float cooldown = 0;

	public bool Shoot(Vector3 start, Vector3 direction)
	{
		//TODO: SHOOT
		if (cooldown <= 0)
		{
			--ammo;
			LazerBeam.CreateBeam(laserPrefab, start, start + direction * range, 0.1f);
			direction = Quaternion.AngleAxis(15f, Vector3.up) * direction;
			LazerBeam.CreateBeam(laserPrefab, start, start + direction * range, 0.1f);
			direction = Quaternion.AngleAxis(-30f, Vector3.up) * direction;
			LazerBeam.CreateBeam(laserPrefab, start, start + direction * range, 0.1f);
			direction = Quaternion.AngleAxis(15f, Vector3.up) * direction;
			direction = Quaternion.AngleAxis(15f, Vector3.right) * direction;
			LazerBeam.CreateBeam(laserPrefab, start, start + direction * range, 0.1f);
			direction = Quaternion.AngleAxis(-30f, Vector3.right) * direction;
			LazerBeam.CreateBeam(laserPrefab, start, start + direction * range, 0.1f);
			cooldown = 1.25f;
		}
		return false;
	}

	public bool Reload()
	{
		if (ammo == 10)
			return false;
		ammo = 10;
		return true;
	}

	public void Update()
	{
		if (cooldown > 0)
		{
			cooldown -= Time.deltaTime;
		}
	}

	public void SetCooldown(float amt)
	{
		cooldown = amt;
	}

	public float GetAmmoPercent()
	{
		return ((float)ammo) * div;
	}

	public Gun.GunType GetGunType()
	{
		return Gun.GunType.Shotgun;
	}
}
