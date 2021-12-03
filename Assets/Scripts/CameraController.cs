using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform following;
    public Vector3 orginOffset;
    public Vector3 orginOffsetWithRotation;
    public Vector2 clipPlaneOffset;
    public float distance;
    public float sensitivity;
    public Vector2 rotXMinMax;

    private Camera cam;
    private Vector3 orginOffsetWithRotationCurrent = Vector3.zero;
    private Vector3 orginOffsetWithRotationVel = Vector3.zero;
    private HUDManager hud;

    // Start is called before the first frame update
    void Start() {
        cam = GetComponentInChildren<Camera>();
        hud = GetComponent<HUDManager>();
		if (!hud.mobileMode)
		{
        	Cursor.lockState = CursorLockMode.Locked;
        	Cursor.visible = false;
		}
        orginOffsetWithRotationCurrent = transform.rotation * orginOffsetWithRotation;
    }

	Vector2 lookInput = Vector2.zero;

    // LateUpdate is called once per frame at the end of everything
    void LateUpdate() {
		if (hud.mobileMode) {
        	if (Input.GetKeyDown(KeyCode.Escape)) {
            	Cursor.lockState = CursorLockMode.None;
            	Cursor.visible = true;
			}
        }

        if (Cursor.visible && !hud.mobileMode) {
            if (Input.GetMouseButton(0) && !IconStorage.changing) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else {
            float rotMod = transform.rotation.eulerAngles.x <= 90 ? transform.rotation.eulerAngles.x + 360 : transform.rotation.eulerAngles.x;
            if (hud.mobileMode) {
				lookInput.Set(hud.rightJoystick.Horizontal, hud.rightJoystick.Vertical);
			}
			else {
				lookInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			}
            transform.rotation = Quaternion.Euler(Mathf.Clamp(rotMod - lookInput.y * sensitivity, rotXMinMax.x, rotXMinMax.y),
                transform.rotation.eulerAngles.y + lookInput.x * sensitivity, 0);
        }

        //Info provided is Camera FoV/Aspect Ratio/Near Clip Plane, get topleft of Near Clip Plane's local position in rectagular cordinates
        float calc1 = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * cam.nearClipPlane + clipPlaneOffset.x;    //see if this can be called only if smth changes to save on calcs
        float calc2 = calc1 * cam.aspect + clipPlaneOffset.x;
        float calc3 = cam.nearClipPlane - clipPlaneOffset.y;
        float calcDist = distance;
        int layer = ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit ray;

        Vector3 orginOfAll = following.position + orginOffset;
        Vector3 orginOffsetWithRotationTemp = transform.rotation * orginOffsetWithRotation;

        foreach (int dir in new int[3] { 1, -1, 0 })
            if (dir == 0 || !Physics.Linecast(orginOfAll, orginOfAll + (orginOffsetWithRotationTemp + transform.right * calc2 * 0.5f) * dir, out ray, layer)) {
                orginOffsetWithRotationTemp = orginOffsetWithRotationTemp * dir;
                break;
            }

        orginOffsetWithRotationCurrent = Vector3.SmoothDamp(orginOffsetWithRotationCurrent, orginOffsetWithRotationTemp, ref orginOffsetWithRotationVel, 0.05f, 1f, Time.fixedDeltaTime);
        orginOfAll += orginOffsetWithRotationCurrent;

        //getting Hypotnuse dist
        foreach (Vector3 vec in new Vector3[4] { new Vector3(calc2, calc1, calc3), new Vector3(-calc2, calc1, calc3), new Vector3(calc2, -calc1, calc3), new Vector3(-calc2, -calc1, calc3) })
            if (Physics.Linecast(orginOfAll, orginOfAll + transform.rotation * (Vector3.back * distance + vec), out ray, layer) && ray.distance < calcDist)
                calcDist = ray.distance;

        following.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.position = orginOfAll - transform.forward * (Mathf.Cos(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * calcDist);
    }
}
