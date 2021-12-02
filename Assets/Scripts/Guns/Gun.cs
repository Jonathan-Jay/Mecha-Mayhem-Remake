using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Gun
{
	enum GunType {
		Empty = 0,
		Pistol,
		Rifle,
		MachineGun,
		Shotgun,
		Cannon,
		HealPack,
	}


    int Shoot(Vector3 start, Vector3 direction, Vector3 muzzel);
	bool Reload();
	bool GetAuto();
	void Update();
	void SetCooldown(float amt);
	float GetAmmoPercent();
	GunType GetGunType();
}
