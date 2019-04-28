using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSwitch : MonoBehaviour {

	// Public Fields
	public GameObject switchObject;
	public float angleOffset;

	public AudioSource source;
	public AudioClip onSound;
	public AudioClip offSound;

	// Internal Fields
	private float _restPoint;
	private bool _enabled;

	void Start() {
		_restPoint = switchObject.transform.localRotation.x;
		HandleTransform();
	}

	public void ToggleSwitch() {
		HandleTransform();
		PlaySound();
	}

	// Internal Methods
	private void HandleTransform() {
		float direction = 1.0F;
		_enabled = !_enabled;
		if (_enabled) {
			direction *= -1;
		}
		switchObject.transform.localRotation = Quaternion.Euler(_restPoint + angleOffset * direction, 0.0F, 0.0F);
	}

	private void PlaySound() {
		AudioClip sound = _enabled ? onSound : offSound;
		source.PlayOneShot(sound);
	}
}
