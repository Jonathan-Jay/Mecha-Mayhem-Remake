using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteThis : MonoBehaviour
{
	public GameObject cube;
	public Transform target;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) {
			LazerBeam.CreateBeam(cube, transform.position, target.position, 0.5f);
		}
    }
}
