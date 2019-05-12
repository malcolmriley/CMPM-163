using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class CycleController : MonoBehaviour {

	// Public References
	public ParticleSystem particles;
	public Material armorMaterial;
	public Material overlayMaterial;
	public float spawnRate = 10.0F;

	// Private References
	private Animation _animation;
	private float _progress;

	void Start() {
		_animation = GetComponent<Animation>();
		UpdateColors();
	}

	void Update() {
		_progress += Time.deltaTime * Random.Range(0.7F, 1.2F);
		if (ShouldSpawn()) {
			_animation.Play();
			UpdateColors();
			_progress = 0.0F;
		}
	}

	// Internal Methods
	private void UpdateColors() {
		// Initialize New Color
		Color newColor = GetColor();

		// Set Particle Parameters
		var module = particles.colorOverLifetime;
		module.color = newColor;

		// Set Armor Color
		armorMaterial.color = newColor;
		overlayMaterial.color = newColor;

		// Reset Particle System
		particles.Stop();
		particles.Play();
	}

	private Color GetColor() {
		return Random.ColorHSV(0.0F, 1.0F, 0.85F, 1.0F, 0.4F, 0.8F);
	}

	private bool ShouldSpawn() {
		// return (Input.GetKeyDown(KeyCode.Space) && !_animation.isPlaying);
		return (_progress > spawnRate);
	}
}
