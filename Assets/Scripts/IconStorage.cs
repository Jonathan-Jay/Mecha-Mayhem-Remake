using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconStorage : MonoBehaviour
{
	public GameObject laserPrefab;
	public Sprite[] tempGunIcons;
	public static Sprite[] gunIcons;
	public Sprite[] tempDigits;
	public static Sprite[] digits;
	public GameObject[] players;

    void Start()
    {
        Pistol.laserPrefab = laserPrefab;
        Rifle.laserPrefab = laserPrefab;
		gunIcons = tempGunIcons;
		digits = tempDigits;
		TrackingFiring.players = players;
		Destroy(gameObject);
    }
}
