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
		Cannon,
		Shotgun,
	}


    bool Shoot(Vector3 start, Vector3 direction);
	void Reload();
	void Update();
	void SetCooldown(float amt);
	float GetAmmoPercent();
	GunType GetGunType();
}