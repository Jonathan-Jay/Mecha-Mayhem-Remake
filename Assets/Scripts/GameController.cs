using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject onScreenControls;
    public Joystick leftJoystick;
    public Joystick rightJoystick;

    // Start is called before the first frame update
    void Start() {
        switch (Application.platform) {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                onScreenControls.SetActive(true);
                break;
            default:
                onScreenControls.SetActive(false);
                break;
        }
    }
}
