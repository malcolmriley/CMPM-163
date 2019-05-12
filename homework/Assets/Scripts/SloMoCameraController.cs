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
	[Range(0.01F, 20.0F)]
	public float zoomRate = 0.5F;
	public float zoomFOV = 40.0F;

	// Internal Fields
	private Vector3 _defaultForward;
	private float _defaultFOV;
	private float _lerp;

	public void Start() {
		_defaultFOV = controlledCameras[0].fieldOfView;
		_defaultForward = transform.forward;
	}

	void Update() {
		HandleLook(Input.mousePosition);
	}

	private void HandleLook(Vector3 mousePosition) {
		Quaternion desiredTarget = Quaternion.identity;

		Vector3 toTarget = lookTarget.transform.position - transform.position;
		bool angleGood = Vector3.Angle(toTarget, _defaultForward) < Mathf.Min(maxVerticalLook, maxHorizontalLook);
		bool keyPressed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
		if (angleGood && keyPressed) {
			desiredTarget = Quaternion.LookRotation(toTarget, transform.up);
			_lerp = Mathf.Clamp01(_lerp + zoomRate * Time.deltaTime);
		}

		else {
			// Reset Variables
			_lerp = Mathf.Clamp01(_lerp - zoomRate * Time.deltaTime);

			// Follow Mouse View
			float horizontal = (mousePosition.x / Screen.width);
			float vertical = (mousePosition.y / Screen.height);

			float verticalAngle = -1.0F * Mathf.SmoothStep(-maxVerticalLook, maxVerticalLook, vertical);
			float horizontalAngle = Mathf.SmoothStep(-maxHorizontalLook, maxHorizontalLook, horizontal);
			desiredTarget = Quaternion.Euler(verticalAngle, horizontalAngle, 0.0F);
		}

		Time.timeScale = Mathf.Lerp(0.3F, 1.0F, 1 - _lerp);
		SetFOV(Mathf.Lerp(zoomFOV, _defaultFOV, 1 - _lerp));
		UIText.color = new Color(UIText.color.r, UIText.color.g, UIText.color.b, 1 - _lerp);
		float lookLerp = (angleGood && keyPressed) ? _lerp : 1 - _lerp;
		transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredTarget, lookLerp);
	}

	private void SetFOV(float fieldOfView) {
		foreach (Camera iteratedCamera in controlledCameras) {
			iteratedCamera.fieldOfView = fieldOfView;
		}
	}
}
