using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSystemController : MonoBehaviour {

	// Public Fields
	public FFTAnalyzer analyzer;
	public Gradient lightColorGrade;

	[Header("Internal Use Only")]
	public Light vfxLight;
	public SpriteRenderer sigilSprite;
	public ParticleSystem mistSystem;
	public ParticleSystem glowSystem;
	public ParticleSystem sparkleSystem;
	public ParticleSystem sigilSystem;

	void Start() {
		
	}

	void Update() {
		
	}
}
