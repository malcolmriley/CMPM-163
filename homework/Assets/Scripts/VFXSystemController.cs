using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSystemController : MonoBehaviour {

	// Public Fields
	public FFTAnalyzer analyzer;
	public Gradient lightColorGrade;

	[Header("Internal Use Only")]
	public Light light;
	public ParticleSystem smokeSystem;
	public ParticleSystem glowSystem;
	public ParticleSystem electricSystem;
	public ParticleSystem sigilSystem;

	void Start() {
		
	}

	void Update() {
		
	}
}
