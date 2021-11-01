using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
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
			cooldown = 1.5f;
		}
		return false;
	}

	public void Reload()
	{
		ammo = 10;
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
		return Gun.GunType.Rifle;
	}
}
