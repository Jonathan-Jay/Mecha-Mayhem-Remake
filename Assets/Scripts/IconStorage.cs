using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class IconStorage : MonoBehaviour
{
	public GameObject laserPrefab;
	public Sprite[] tempGunIcons;
	public Sprite[] tempHealIcons;
	public static Sprite[] gunIcons;
	public GameObject[] tempGuns;
	public static GameObject[] gunPrefabs;
	public Sprite[] tempDigits;
	public static Sprite[] digits;
	public GameObject[] players;
	public List<GameObject> turrets;
	public PlayerController player;
	public static bool changing = false;

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
		//Destroy(gameObject);
		changing = false;
    }
	private void Update() {
		for (int i = 0; i < turrets.Count;) {
			if (turrets[i] == null)
				turrets.RemoveAt(i);
			else
				++i;
		}
		if (turrets.Count == 0) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			changing = true;
			SceneManager.LoadScene("Win");
		}
		if (player.dead()) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			changing = true;
			SceneManager.LoadScene("Lose");
		}
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
