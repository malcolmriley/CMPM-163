using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraController : MonoBehaviour {

	public float maxVerticalLook;
	public float maxHorizontalLook;

	void Update() {
		HandleLook(Input.mousePosition);
	}

	private void HandleLook(Vector3 mousePosition) {
		float horizontal = (mousePosition.x / Screen.width);
		float vertical = (mousePosition.y / Screen.height);

		float verticalAngle = -1.0F * Mathf.SmoothStep(-maxVerticalLook, maxVerticalLook, vertical);
		float horizontalAngle = Mathf.SmoothStep(-maxHorizontalLook, maxHorizontalLook, horizontal);
		transform.localRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0.0F);
	}
}
