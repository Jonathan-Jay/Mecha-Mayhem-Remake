using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectButton : MonoBehaviour
{
    public bool touched = false;
    private bool lastTouched = false;
	Vector2 min = Vector2.zero;
	Vector2 max = Vector2.zero;
	int lastSize = 0;

	private void Start() {
		RectTransform trans = GetComponent<RectTransform>();
		Vector2 scale = Camera.main.pixelRect.size / trans.GetComponentInParent<CanvasScaler>().referenceResolution;
		min = Camera.main.pixelRect.size * trans.anchorMin
			+ (trans.anchoredPosition - trans.pivot * trans.rect.size) * scale;
		max = Camera.main.pixelRect.size * trans.anchorMax
			+ (trans.anchoredPosition + (Vector2.one - trans.pivot) * trans.rect.size) * scale;
	}

    // Update is called once per frame
    void Update()
	{
		lastTouched = touched;
		if (Input.touchCount > 0) {
			if (lastSize != Input.touchCount) {
				Vector2 pos = Vector2.zero;
				for (int i = 0; i < Input.touchCount; ++i) {
					touched = touched || AABB(Input.touches[i].position);
				}
			}
		}
		else if (touched) {
			touched = false;
		}
		lastSize = Input.touchCount;
    }

	bool AABB(Vector2 pos) {
		return (
			min.x < pos.x && max.x > pos.x &&
			min.y < pos.y && max.y > pos.y
		);
	}

	public bool GetDown() {
		return touched && !lastTouched;
	}

	public bool GetUp() {
		return !touched && lastTouched;
	}
}
