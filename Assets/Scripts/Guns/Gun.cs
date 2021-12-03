using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun
{
	public enum GunType {
		Empty = 0,
		Pistol,
		Rifle,
		MachineGun,
		Shotgun,
		Cannon,
		HealPack,
	}

    public abstract int Shoot(Vector3 start, Vector3 direction, Vector3 muzzel, AudioSource pew);
	public abstract bool Reload();
	public abstract bool GetAuto();
	public abstract void Update();
	public abstract void SetCooldown(float amt);
	public abstract float GetAmmoPercent();
	public abstract GunType GetGunType();

	public int ShootLaser(Vector3 start, Vector3 direction, out Vector3 endPos, float range, float damage) {
		int killed = 0;
		RaycastHit hit;
		endPos = start + direction * range;
		if (Physics.Raycast(start, direction, out hit, range))
		{
			if (hit.transform.CompareTag("Turret"))
			{
				if (hit.transform.GetComponent<TrackingFiring>().TakeDamage(damage))
				{
					++killed;
				}
				++killed;
			}
			endPos = hit.point;
		}
		return killed;
	}
}
