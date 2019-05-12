using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SloMoCameraController : MonoBehaviour {

	// Public Fields
	public Text UIText;
	public float maxVerticalLook;
	public float maxHorizontalLook;
	public List<Camera> controlledCameras;
	public Transform lookTarget;
	public float zoomFOV = 40.0F;

	// Internal Fields
	private float _defaultFOV;

	public void Start() {
		_defaultFOV = controlledCameras[0].fieldOfView;
	}

	void Update() {
		HandleLook(Input.mousePosition);
	}

	private void HandleLook(Vector3 mousePosition) {
		transform.localRotation = Quaternion.Euler(0.0F, 0.0F, 0.0F);

		Vector3 toTarget = lookTarget.transform.position - transform.position;
		bool angleGood = Vector3.Angle(toTarget, transform.forward) < Mathf.Min(maxVerticalLook, maxHorizontalLook);
		bool keyPressed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
		if (angleGood && keyPressed) {
			transform.LookAt(lookTarget);
			SetFOV(zoomFOV);
			UIText.enabled = false;
			Time.timeScale = 0.3F;
		}

		else {
			// Reset Variables
			SetFOV(_defaultFOV);
			UIText.enabled = true;
			Time.timeScale = 1.0F;

			// Follow Mouse View
			float horizontal = (mousePosition.x / Screen.width);
			float vertical = (mousePosition.y / Screen.height);

			float verticalAngle = -1.0F * Mathf.SmoothStep(-maxVerticalLook, maxVerticalLook, vertical);
			float horizontalAngle = Mathf.SmoothStep(-maxHorizontalLook, maxHorizontalLook, horizontal);
			transform.localRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0.0F);
		}
	}

	private void SetFOV(float fieldOfView) {
		foreach (Camera iteratedCamera in controlledCameras) {
			iteratedCamera.fieldOfView = fieldOfView;
		}
	}
}
