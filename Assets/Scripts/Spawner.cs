using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Animator spawnerAnimator;
	public Transform gunMesh;

	public List<Gun.GunType> options;
	public float delayMax = 30;
	public float delayMin = 10;

	[SerializeField]
	private float counter = 0;
	private Gun.GunType current = Gun.GunType.Empty;

    // Start is called before the first frame update
    void Start()
    {
		//remove all invalids
        options.RemoveAll(delegate(Gun.GunType current) {
			return current == Gun.GunType.Empty;
			});
		if (counter == 0) {
			counter = Random.Range(delayMin, delayMax);
		}
		if (gunMesh.transform.childCount > 0) {
			Destroy(gunMesh.transform.GetChild(0).gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (options.Count == 0)	return;

		if (current == Gun.GunType.Empty) {
        	counter -= Time.deltaTime;
			if (counter <= 0) {
				current = options[Random.Range(0, options.Count)];
				Instantiate(IconStorage.gunPrefabs[(int)current], gunMesh);
				spawnerAnimator.SetBool("open", true);
			}
		}
		else {
			counter += Time.deltaTime;
			gunMesh.localPosition = Vector3.up + Vector3.up * Mathf.Sin(counter) * 0.25f;
			gunMesh.rotation = Quaternion.Euler(0f, 120f * counter, 0f);
		}
    }

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player")) {
			if (current == Gun.GunType.HealPack) {
				if (other.GetComponent<PlayerController>().PickUpHealPack()) {
					current = Gun.GunType.Empty;
					counter = Random.Range(delayMin, delayMax);
					if (gunMesh.transform.childCount > 0) {
						Destroy(gunMesh.transform.GetChild(0).gameObject);
					}
					spawnerAnimator.SetBool("open", false);
				}
			}
			else if (current != Gun.GunType.Empty) {
				if (other.GetComponent<PlayerController>().PickUpWeapon(current)) {
					current = Gun.GunType.Empty;
					counter = Random.Range(delayMin, delayMax);
					if (gunMesh.transform.childCount > 0) {
						Destroy(gunMesh.transform.GetChild(0).gameObject);
					}
					spawnerAnimator.SetBool("open", false);
				}
			}
		}
	}
}
