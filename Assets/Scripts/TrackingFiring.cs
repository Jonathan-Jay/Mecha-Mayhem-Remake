using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingFiring : MonoBehaviour
{
	public AudioSource pew;
	public AudioSource hit;
	public float health = 100f;
    public float turnSpeed = 1f;
    public float scanFOV = 135f;
    public float scanRotationAngle = 135f;
    public float snapSpeed = 1f;
    public float scanRange = 100f;
    public float delayPerShot = 0.2f;
    public float damage = 1f;

	public Transform body;
	public Transform muzzel;
	public Transform barrel;
	public float barrelRotSpeed = 720f;
	private float barrelRot = 0;

    public GameObject laserStyle;

    public float verticalViewRange = 30f;

    bool isTargeting = false;
    GameObject target;

    //For testing purposes only, what is better is to have a global player list that generates on game launch
    public static GameObject[] players;
    Quaternion startRotation;
    Quaternion endRotation;
    float t = 0f;
    float shootCounter = 0f;
    float tRotation = 0f;
    bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = body.localRotation * Quaternion.AngleAxis(scanRotationAngle * -0.5f, Vector3.up);
        endRotation = startRotation * Quaternion.AngleAxis(scanRotationAngle * 0.5f, Vector3.up);
    }

    // Update is called once per 20ms
    //Create a ray for better tracking
    void FixedUpdate()
    {
        //////////////////////
        /////DEBUGGING //////
        ////////////////////
        //List<Vector3> rayVerts = new List<Vector3>();
        //rayVerts.Add(Vector3.forward);
        //rayVerts.Add(Quaternion.AngleAxis(scanFOV / 2f, Vector3.left) * Vector3.forward);
        //rayVerts.Add(Quaternion.AngleAxis(scanFOV / 2f, Vector3.right) * Vector3.forward);
        //I know this is stupid, remove this after
         //Debug.DrawRay(muzzel.position, body.TransformDirection(Vector3.forward) * scanRange, Color.yellow);
         //Debug.DrawRay(muzzel.position, body.TransformDirection(rayVerts[1]) * scanRange, Color.yellow);
         //Debug.DrawRay(muzzel.position, body.TransformDirection(rayVerts[2]) * scanRange, Color.yellow);

		bool targetted = false;
        foreach (GameObject player in players)
        {

           	RaycastHit hit;
           	Vector3 direction = (player.transform.position + Vector3.up - muzzel.position).normalized;
           	if (Physics.Raycast(muzzel.position, direction, out hit, scanRange))
           	{
               if(hit.transform.tag == "Player")
               {
                   	if (Vector3.Angle(body.forward, direction) < scanFOV / 2f)
                   	{
						bool overwriteCurrent = true;
						if (target != null) {
							overwriteCurrent = (target.transform.position + Vector3.up - muzzel.position).magnitude >
					   				(hit.transform.position + Vector3.up - muzzel.position).magnitude;
						}
					   	if (overwriteCurrent)
						{
                        	isTargeting = true;
                        	target = hit.transform.gameObject;
						}
						targetted = true;
                   	}
               }
			}
        }

		if (!targetted)
		{
			isTargeting = false;
			tRotation = 0;
			target = null;
		}
    }
        //Priority is on the forward vector, so once it's tracking, it can continue to fire the weapon
        //Also gives a small window to players before bullets start to hit them
    //     foreach (Vector3 item in rayVerts)
    //     {
    //         RaycastHit hit;
    //         if (Physics.Raycast(transform.position, transform.TransformDirection(item), out hit, Mathf.Infinity))
    //         {
    //             if(hit.transform.tag == "Player")
    //             {
    //                 isTargeting = true;
    //                 target = hit.transform.gameObject;
    //                 //Fire the weapon
    //                 LazerBeam.CreateBeam(laserStyle, transform.position, hit.transform.position, 0.1f);
    //                 break;
    //             }
    //             else
    //             {
    //                 isTargeting = false;
    //                 target = null;
    //             }
    //         }
    //     }

    // }
    void Update()
    {
        //If I am Lerping I ssupect I can use Coroutines. Otherwise not for this week
        //TODO: Need to fix the angle at which the turret is pointing after firing.
        if(!isTargeting)
        {
            //currentAngle = transform.rotation.eulerAngles.y;
            // transform.rotation *= Quaternion.AngleAxis(turnSpeed * Time.deltaTime, Vector3.up);
            if(isMoving)
            {
                body.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
                t += Time.deltaTime * turnSpeed; //adjust the speed here
            }
            if (t >= 0.999f || t <= 0.001f)
            {
                t = 0.01f;
                isMoving = false;
                StartCoroutine(RotationPoints());
            }
        }
        else 
        {
			barrelRot += Time.deltaTime * tRotation * barrelRotSpeed;
			if (barrelRot >= 360f) {
				barrelRot -= 360f;
			}
			barrel.localRotation = Quaternion.AngleAxis(barrelRot, Vector3.up);

            //Lower this number to reduce the delay between the turret aiming and shooting
            if(tRotation < 0.95f)
            {
                Vector3 lookDirection =  (target.transform.position + Vector3.up - muzzel.position).normalized;
                body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(lookDirection), tRotation);
                tRotation += Time.deltaTime * snapSpeed;
            }
            else 
            {
                body.LookAt(target.transform.position + Vector3.up);
				shootCounter += Time.deltaTime;
				if (shootCounter > delayPerShot) {
					shootCounter -= delayPerShot;
                	LazerBeam.CreateBeam(laserStyle, muzzel.position, target.transform.position + Vector3.up, 0.1f);
					pew.Play();
					target.GetComponent<PlayerController>().TakeDamage(damage);
				}
            }
        }
    }
    // IEnumerator CameraReset()
    // {
    //     Vector3 lookDirection =  (target.transform.position - transform.position).normalized;
    //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), tRotation);
    //     tRotation += Time.deltaTime * snapSpeed;
    //     yield return null;
    // }
    IEnumerator RotationPoints()
    {
        yield return new WaitForSeconds(2f);
        Quaternion holdSwap = startRotation;
        startRotation = endRotation;
        endRotation = holdSwap;
        isMoving = true;
    }

	public bool TakeDamage(float damage) {
		health -= damage;
		if (health <= 0) {
			Destroy(gameObject);
			return true;
		}
		hit.Play();
		return false;
	}
}

