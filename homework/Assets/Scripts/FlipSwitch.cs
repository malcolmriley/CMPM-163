using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSwitch : MonoBehaviour {

	// Public Fields
	public GameObject switchObject;
	public float angleOffset;

	// Internal Fields
	private float _restPoint;
	private bool _enabled;

	void Start() {
		_restPoint = switchObject.transform.localRotation.x;
		ToggleSwitch();
	}

	public void ToggleSwitch() {
		float direction = 1.0F;
		_enabled = !_enabled;
		if (_enabled) {
			direction *= -1; 
		}
		switchObject.transform.localRotation = Quaternion.Euler(_restPoint + angleOffset * direction, 0.0F, 0.0F);
	}
}
