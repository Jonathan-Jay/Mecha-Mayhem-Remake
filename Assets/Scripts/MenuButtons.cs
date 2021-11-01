using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayPress()
    {
        SceneManager.LoadScene("JJ's Character importing");
        
    }
    public void ExitPress()
    {
    	Application.Quit();
    }
}
