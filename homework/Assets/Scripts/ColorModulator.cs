using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class ColorModulator : MonoBehaviour {

	// Public Fields
	public Gradient colorGrade;
	[Range(0.01F, 3.0F)]
	public float timeScale;

	// Internal Fields
	private Light _light;

	public void Start() {
		_light = GetComponent<Light>();
	}

	// Update is called once per frame
	void Update() {
		float grade = 0.5F * (1 + Mathf.Sin(timeScale * Time.timeSinceLevelLoad));
		_light.color = colorGrade.Evaluate(grade);
	}
}
