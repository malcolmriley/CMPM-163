using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveTo : MonoBehaviour {

	// Public Fields
	public Transform moveTarget;
	public float moveTime = 15.0F;

	// Internal Fields
	private Vector3 _startPosition;
	private float _progress;

	public void Start() {
		_startPosition = transform.localPosition;
	}

	public void Update() {
		_progress = Mathf.Clamp01(_progress + (Time.deltaTime / moveTime));
		transform.localPosition = Vector3.Lerp(_startPosition, moveTarget.localPosition, _progress);
	}
}
