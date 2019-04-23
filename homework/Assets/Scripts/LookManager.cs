using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LookManager : MonoBehaviour {

	// Public References
	public GameObject cameraDolly;
	public Transform defaultTransform;

	// Public Fields
	public float maxVerticalLook;
	public float maxHorizontalLook;
	public float lookSpeed;

	// Internal References
	private Camera _camera;
	private Vector3 _lastPosition;
	private Quaternion _lastRotation;

	// Internal Fields
	private Transform _lookTarget;
	private bool _isInteracting;
	private float _lookProgress;

	public void Start() {
		_camera = GetComponent<Camera>();
		UpdateLastTransform();
	}

	public void Update() {
		Vector3 mousePosition = Input.mousePosition;

		HandleLook(mousePosition);
		HandleInteract(mousePosition);
	}

	private void HandleLook(Vector3 mousePosition) {
		float horizontal = (mousePosition.x / Screen.width);
		float vertical = (mousePosition.y / Screen.height);

		float verticalAngle = -1.0F * Mathf.SmoothStep(-maxVerticalLook, maxVerticalLook, vertical);
		float horizontalAngle = Mathf.SmoothStep(-maxHorizontalLook, maxHorizontalLook, horizontal);
		_camera.transform.localRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0.0F);
	}

	private void HandleInteract(Vector3 mousePosition) {
		// Handle Mouseover and Click
		Ray ray = _camera.ScreenPointToRay(mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 20.0F)) {
			LookTarget target = hit.collider.gameObject.GetComponent<LookTarget>();
			if (!(target is null)) {
				if (Input.GetMouseButtonDown(0) && !_isInteracting) {
					SetLookTarget(target.cameraMoveTarget);
				}
				else {
					// Mouseover
					// print("mouseover " + hit.collider.gameObject.name);
				}
			}
		}

		// Handle Camera Pan
		if (_isInteracting) {
			if (_lookProgress >= 1.0F) {
				UpdateLastTransform();
				_isInteracting = false;
			}
			else {
				_lookProgress = Mathf.Clamp01(_lookProgress + lookSpeed);
				cameraDolly.transform.position = Vector3.Slerp(_lastPosition, _lookTarget.position, _lookProgress);
				cameraDolly.transform.rotation = Quaternion.Slerp(_lastRotation, _lookTarget.rotation, _lookProgress);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Space)) {
			SetLookTarget(defaultTransform);
		}
	}

	private void SetLookTarget(Transform passedTransform) {
		_isInteracting = true;
		_lookProgress = 0.0F;
		_lookTarget = passedTransform;
	}

	private void UpdateLastTransform() {
		_lastPosition = cameraDolly.transform.position;
		_lastRotation = cameraDolly.transform.rotation;
	}
}
