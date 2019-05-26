using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class VFXSystemController : MonoBehaviour {

	// Public Fields
	[Header("FFT Analyzer Properties")]
	public FFTAnalyzer analyzer;
	public Vector2 bassRange;
	public Vector2 midRange;
	public Vector2 trebleRange;

	[Header("Output Modulation")]
	public AnimationCurve normalizer;
	public Gradient lightColorGrade;
	public int burstCount = 50;

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
		// Precalculate values
		float bass = GetBass();
		float midrange = GetMidrange();
		float treble = GetTreble();

		// Update Light
		vfxLight.color = lightColorGrade.Evaluate(bass);

		// Update Glow
		glowSystem.Emit((int)(burstCount * midrange));

		// Update Sigil
		sigilSprite.color = lightColorGrade.Evaluate(bass);
		sigilSystem.Emit((int)(burstCount * treble));
	}

	// Internal Methods
	private float GetBass() {
		return AverageRange(analyzer.Spectrum, bassRange);
	}

	private float GetMidrange() {
		return AverageRange(analyzer.Spectrum, midRange);
	}

	private float GetTreble() {
		return AverageRange(analyzer.Spectrum, trebleRange);
	}

	private float AverageRange(float[] array, Vector2 range) {
		int min = Mathf.RoundToInt(range.x * (array.Length - 1));
		int max = Mathf.RoundToInt(range.y * (array.Length - 1));

		if (max < min) return 0;
		if (max == min) return array[max];

		float average = 0.0F;
		for (int index = min; index <= max; index += 1) {
			float normalize = normalizer.Evaluate((float)index / (float)array.Length);
			average += array[index] * normalize;
		}
		return Mathf.Clamp01(average / (max - min + 1));
	}
}
