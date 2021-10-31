using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	public Image tensDigit;
	public Image onesDigit;
	public Image offhandWeapon;
	public Image mainWeapon;
	public Image healthPickup;
	public Slider healthBar;
	public Slider dashBar;
	public Slider ammoBar;
	public Color dashFull;
	public Color dashCharging;
	public Sprite[] healthPickupSprite;

	public float healthMax = 100;
	public float dashMax = 30;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = healthMax;
        healthBar.value = healthMax;
        dashBar.maxValue = dashMax;
        dashBar.value = dashMax;
    }

	public void SetScore(int score) {
		if (score <= 0) {
			tensDigit.enabled = false;
			onesDigit.sprite = IconStorage.digits[0];
		}
		if (score < 100) {
			int tens = score / 10;
			int ones = score - tens * 10;
			tensDigit.enabled = score > 9;

			tensDigit.sprite = IconStorage.digits[tens];
			onesDigit.sprite = IconStorage.digits[ones];
		}
		else {
			tensDigit.sprite = IconStorage.digits[9];
			onesDigit.sprite = IconStorage.digits[9];
		}
	}

	public void SetHealth(float health) {
		healthBar.value = health;
	}

	public void SetDash(float dash) {
		dashBar.value = dash;
		dashBar.fillRect.GetComponent<Image>().color = dash == dashMax ? dashFull : dashCharging;
	}

	public void SetAmmoBar(float percentage)
	{
		ammoBar.value = percentage;
	}

	public void SetMainWeapon(Sprite newImage) {
		mainWeapon.sprite = newImage;
	}

	public void SetOffhandWeapon(Sprite newImage) {
		offhandWeapon.sprite = newImage;
	}

	public void SwapWeapons() {
		Sprite temp = offhandWeapon.sprite;
		offhandWeapon.sprite = mainWeapon.sprite;
		mainWeapon.sprite = temp;
	}

	public void SetHealthPickup(int state) {
		if (state >= 0 && state <= healthPickupSprite.Length) {
			healthPickup.enabled = true;
			healthPickup.sprite = healthPickupSprite[state];
		}
		else {
			healthPickup.enabled = false;
		}
	}
}
