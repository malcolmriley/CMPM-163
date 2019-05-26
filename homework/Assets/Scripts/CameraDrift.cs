using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrift : MonoBehaviour {

	// Public Fields
	public float maxPitch = 2.0F;
	public float maxYaw = 2.0F;
	public float timeScale = 0.1F;

	// Internal Fields
	private Quaternion _startRotation;

	void Start() {
		_startRotation = transform.localRotation;
	}

	void Update() {
		float pitch = GetXNoiseModulation(maxPitch);
		float yaw = GetYNoiseModulation(maxYaw);
		transform.localRotation = _startRotation * Quaternion.Euler(0.0F, pitch, yaw);
	}

	// Internal Methods
	private float GetXNoiseModulation(float value) {
		return GetNoiseModulation(value, GetScaledTime(), 0.0F);
	}

	private float GetYNoiseModulation(float value) {
		return GetNoiseModulation(value, 0.0F, GetScaledTime());
	}

	private float GetNoiseModulation(float value, float xParam, float yParam) {
		return value * (0.5F - Mathf.PerlinNoise(xParam, yParam)) * 2.0F;
	}

	private float GetScaledTime() {
		return Time.timeSinceLevelLoad * timeScale;
	}
}
