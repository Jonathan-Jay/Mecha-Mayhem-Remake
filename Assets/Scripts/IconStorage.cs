using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconStorage : MonoBehaviour
{
	public GameObject laserPrefab;
	public Sprite[] tempGunIcons;
	public static Sprite[] gunIcons;
	public GameObject[] tempGuns;
	public static GameObject[] gunPrefabs;
	public Sprite[] tempDigits;
	public static Sprite[] digits;
	public GameObject[] players;

    void Start()
    {
        Pistol.laserPrefab = laserPrefab;
        Rifle.laserPrefab = laserPrefab;
        Shotgun.laserPrefab = laserPrefab;
        Minigun.laserPrefab = laserPrefab;
		gunIcons = tempGunIcons;
		gunPrefabs = tempGuns;
		digits = tempDigits;
		TrackingFiring.players = players;
		Destroy(gameObject);
    }

	public static Gun GetGunFromType(Gun.GunType type) {
		switch (type) {
			case Gun.GunType.Pistol:	return new Pistol();
			case Gun.GunType.Rifle:		return new Rifle();
			case Gun.GunType.MachineGun:		return new Minigun();
			case Gun.GunType.Shotgun:		return new Shotgun();
			default:	return null;
		}
	}
}
