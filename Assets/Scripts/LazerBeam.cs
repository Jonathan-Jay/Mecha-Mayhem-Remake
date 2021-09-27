using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LazerBeam : MonoBehaviour
{
	private float beginScale;
	private float length;
	private float counter;
	private float lifeSpan;
	private MeshRenderer mesh;

	// Update is called once per frame
	void Update()
    {
		counter -= Time.deltaTime;
		if (counter <= 0) {
			Destroy(gameObject);
			return;
		}
		float t = counter / lifeSpan;
		Vector3 tempVec = Vector3.one * t * beginScale;
		tempVec.z = length;
		transform.localScale = tempVec;

		Color tempCol = mesh.material.color;
		tempCol.a = t * t;
		mesh.material.color = tempCol;
	}

	//make sure prefab has depth of 1
	static public void CreateBeam(GameObject prefab, Vector3 start, Vector3 end, float lifeSpan)
    {
		Vector3 averagePos = start + (end - start) * 0.5f;
		GameObject instant = Instantiate(prefab, averagePos, Quaternion.identity);
		instant.transform.LookAt(end);

		//give laser for safety
		LazerBeam laser = null;
		if (!instant.TryGetComponent<LazerBeam>(out laser)) {
			laser = instant.AddComponent<LazerBeam>();
		}
		laser.beginScale = instant.transform.localScale.x;
		laser.length = Vector3.Distance(start, end);
		Vector3 tempVec = instant.transform.localScale;
		tempVec.z = laser.length;
		instant.transform.localScale = tempVec;

		laser.lifeSpan = lifeSpan;
		laser.counter = lifeSpan;
		laser.mesh = instant.GetComponent<MeshRenderer>();
	}
}
