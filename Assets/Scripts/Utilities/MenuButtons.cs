using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {
    public void ChangeScene(string scene) {
        SceneManager.LoadScene(scene);
    }
    public void ExitPress() {
    	Application.Quit();
    }
}
