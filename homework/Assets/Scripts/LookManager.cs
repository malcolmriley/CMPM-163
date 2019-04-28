using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LookManager : MonoBehaviour {

	// Public References
	public GameObject cameraDolly;
	public Transform defaultTransform;
	public SpriteRenderer uiComponent;
	public Sprite interactSprite;
	public Sprite exitSprite;
	public LookTarget CurrentTarget { get; private set; }

	// Public Fields
	public float maxVerticalLook;
	public float maxHorizontalLook;
	public float lookSpeed;
	public float fadeSpeed;

	// Internal References
	private Camera _camera;
	private Vector3 _lastPosition;
	private Quaternion _lastRotation;

	// Internal Fields
	private Transform _currentTarget;
	private Transform _lookTarget;
	private bool _isInteracting;
	private float _lookProgress;
	private float _fadeProgress;

	public void Start() {
		_camera = GetComponent<Camera>();
		_currentTarget = defaultTransform;
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
		// Mouseover and Click
		Ray ray = _camera.ScreenPointToRay(mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 20.0F)) {
			LookTarget target = hit.collider.gameObject.GetComponent<LookTarget>();
			if (!(target is null)) {
				uiComponent.sprite = (target.interactOverride == null) ? interactSprite : target.interactOverride;
				FadeTo(fadeSpeed);
				if (Input.GetMouseButtonDown(0) && !_isInteracting) {
					// When Clicked, set move target
					SetLookTarget(target.cameraMoveTarget);
					if (!target.ignoreFocus) {
						if (CurrentTarget != null) CurrentTarget.isCurrentTarget = false;
						CurrentTarget = target;
						target.isCurrentTarget = true;
					}
					target.onLookAt?.Invoke();
				}
				if (_currentTarget.Equals(target.cameraMoveTarget)){
					// If mouseover and looking
					uiComponent.sprite = exitSprite;
					FadeTo(fadeSpeed);
				}
			}
		}
		else {
			FadeTo(-fadeSpeed);
		}

		// Camera Pan
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
		else if (Input.GetMouseButtonDown(1)) {
			SetLookTarget(defaultTransform);
		}
	}

	private void SetLookTarget(Transform passedTransform) {
		if (passedTransform != null) {
			_isInteracting = true;
			_lookProgress = 0.0F;
			_lookTarget = passedTransform;
			_currentTarget = passedTransform;
		}
	}

	private void UpdateLastTransform() {
		_lastPosition = cameraDolly.transform.position;
		_lastRotation = cameraDolly.transform.rotation;
	}

	private void FadeTo(float delta) {
		_fadeProgress = Mathf.Clamp01(_fadeProgress + delta);
		Color color = uiComponent.color;
		color.a = Mathf.SmoothStep(0.0F, 1.0F, _fadeProgress);
		uiComponent.color = color;
	}
}
