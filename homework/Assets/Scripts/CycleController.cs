using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class CycleController : MonoBehaviour {

	// Public References
	public ParticleSystem particles;
	public Material armorMaterial;

	// Private References
	private Animator _animator;
	private Animation _animation;

	void Start() {
		_animator = GetComponent<Animator>();
		_animation = GetComponent<Animation>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) && !_animation.isPlaying) {
			_animation.Play();
			// Initialize New Color
			Color newColor = GetColor();

			// Set Particle Parameters
			var module = particles.colorOverLifetime;
			module.color = newColor;

			// Set Armor Color
			armorMaterial.color = newColor;

			particles.Stop();
			particles.Play();
		}
	}

	// Internal Methods

	private Color GetColor() {
		return Random.ColorHSV(0.0F, 1.0F, 0.85F, 1.0F, 0.4F, 0.8F);
	}
}
