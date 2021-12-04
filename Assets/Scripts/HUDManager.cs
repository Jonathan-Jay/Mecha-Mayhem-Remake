using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	public GameObject mobileHUD;
	public RectTransform weaponAnchor;
	public RectTransform scoreAnchor;
	public Joystick leftJoystick;
	public Joystick rightJoystick;
	public bool mobileMode {get; private set;}

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
		switch (Application.platform)
		{
			//case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
				mobileMode = true;
				weaponAnchor.anchoredPosition = Vector2.one * -10;
				scoreAnchor.anchoredPosition = Vector2.zero;
				scoreAnchor.anchorMin = scoreAnchor.anchorMax = Vector2.up;
				RectTransform newRect = healButton.GetComponent<RectTransform>();
				RectTransform curRect = healthPickup.GetComponent<RectTransform>();
				curRect.anchorMax = curRect.anchorMin = newRect.anchorMax;
				curRect.offsetMax = newRect.offsetMax;
				curRect.offsetMin = newRect.offsetMin;
				break;
			default:
				mobileMode = false;
				break;
		}
		mobileHUD.SetActive(mobileMode);

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
		if (state > 0 && state <= healthPickupSprite.Length) {
			healthPickup.enabled = true;
			healthPickup.sprite = healthPickupSprite[state - 1];
		}
		else {
			healthPickup.enabled = false;
		}
	}

	public void CanShoot(bool val) {
		shootButton.gameObject.SetActive(val);
	}


	public RectButton weaponButton;
	float dropCounter = 0;
	public bool GetDropWeaponInput() {
		if (weaponButton.GetUp()) {
			dropCounter = 0f;
		}
		else if (weaponButton.touched && dropCounter >= 0f) {
			dropCounter += Time.deltaTime;
			if (dropCounter >= 0.5f) {
				dropCounter = -1f;
				return true;
			}
		}
		return false;
	}
	public bool DropWeaponHold(bool inputed) {
		if (dropCounter != 0f && !inputed) {
			dropCounter = 0f;
		}
		else if (inputed && dropCounter >= 0f) {
			dropCounter += Time.deltaTime;
			if (dropCounter > 0.5f) {
				dropCounter = -1f;
				return true;
			}
		}
		return false;
	}

	public RectButton healButton;
	public bool GetHealInput() {
		if (healthPickup.enabled) {
			return healButton.GetDown();
		}
		return false;
	}

	public RectButton shootButton;
	public bool GetShootInput(bool autoMode) {
		if (!shootButton.gameObject.activeInHierarchy)	return false;

		if (autoMode)
			return shootButton.touched;
		return shootButton.GetDown();
	}

	public RectButton dashButton;
	public RectButton jumpButton;
	public RectButton zoomButton;
	bool toggled = false;

	public bool GetZoomInput() {
		if (zoomButton.GetDown()) {
			toggled = !toggled;
			zoomButton.GetComponent<Image>().color = toggled ? Color.white : Color.red;
		}
		return toggled;
	}
}
