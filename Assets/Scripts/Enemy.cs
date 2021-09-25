using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Transform target;
	private Rigidbody rb;
	public float speed = 5;
	public float timing = 1;
	private float counter = 0;

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
		transform.LookAt(target);
		if (timing > 0)
		{
			counter += Time.deltaTime;
			while (counter >= timing)
			{
				counter -= timing;
				Vector3 movement =
					Vector3.up * Random.Range(-1f, 1f) +
					Vector3.right * Random.Range(-1f, 1f) +
					Vector3.forward * Random.Range(0, 2f);
				movement = transform.rotation * movement;
				rb.AddForce(movement * speed, ForceMode.Impulse);
			}
		}
	}
}
