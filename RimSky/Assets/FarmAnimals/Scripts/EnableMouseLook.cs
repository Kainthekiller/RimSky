using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseLook))]
public class EnableMouseLook : MonoBehaviour {

	public MouseLook mouseLook;
	public bool requiresCollider = false;

	void Update() {
		if (Input.GetMouseButtonUp(0)) {
			mouseLook.enabled = false;
		}
		if (Input.GetMouseButtonDown(0)) {
			if (!requiresCollider) {
				mouseLook.enabled = true;
			}
		}
	}
	public void OnMouseDown() {
		if (requiresCollider) {
			mouseLook.enabled = true;
		}
	}
}
