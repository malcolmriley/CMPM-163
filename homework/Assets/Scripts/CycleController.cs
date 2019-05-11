﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class CycleController : MonoBehaviour {

	// Public References
	public ParticleSystem particles;

	// Private References
	private Animator _animator;
	private Animation _animation;

	void Start() {
		_animator = GetComponent<Animator>();
		_animation = GetComponent<Animation>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			_animation.Play();
			particles.Stop();
			particles.Play();
		}
	}
}
