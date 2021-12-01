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
	}


    bool Shoot(Vector3 start, Vector3 direction);
	bool Reload();
	void Update();
	void SetCooldown(float amt);
	float GetAmmoPercent();
	GunType GetGunType();
}
