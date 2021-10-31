using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingFiring : MonoBehaviour
{
    public float turnSpeed = 100f;
    public float scanRadius = 135f;

    public float verticalViewRange = 30f;
    bool isTargeting = false;
    GameObject target;

    Quaternion startRotation;
    Quaternion endRotation;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        endRotation = Quaternion.AngleAxis(scanRadius, Vector3.up);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ///REMOVE LATER THIS WILL BREAK A LOT OF THINGS
        isTargeting = false;
        List<Vector3> rayVerts = new List<Vector3>();
        rayVerts.Add(Vector3.forward);
        rayVerts.Add(Quaternion.AngleAxis(verticalViewRange / 2f, Vector3.left) * Vector3.forward);
        rayVerts.Add(Quaternion.AngleAxis(verticalViewRange / 2f, Vector3.right) * Vector3.forward);




        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1, Color.yellow);
        Debug.DrawRay(transform.position, transform.TransformDirection(rayVerts[1]) * 1, Color.yellow);
        Debug.DrawRay(transform.position, transform.TransformDirection(rayVerts[2]) * 1, Color.yellow);

        //Priority is on the forward vector, so once it's tracking, it can continue to fire the weapon
        //Also gives a small window to players before bullets start to hit them
        foreach (Vector3 item in rayVerts)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(item), out hit, Mathf.Infinity))
            {
                if(hit.transform.tag == "Player")
                {
                    isTargeting = true;
                    target = hit.transform.gameObject;
                    //Fire the weapon
                    break;
                }
                else
                {
                    isTargeting = false;
                    target = null;
                }
            }
        }

    }
    // float t = 0f;
    // float angle = 0f;
    // int tFlip = 0;
    //float currentAngle = 0;
    float t = 0f;
    bool isMoving = true;
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
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                t += Time.deltaTime * 0.1f; //adjust the speed here
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
            //Add smoothing when tracking target
            transform.LookAt(target.transform.position);
        }
    }
    IEnumerator RotationPoints()
    {
        yield return new WaitForSeconds(2f);
        Quaternion holdSwap = startRotation;
        startRotation = endRotation;
        endRotation = holdSwap;
        isMoving = true;
    }
}

