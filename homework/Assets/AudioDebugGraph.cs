using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AudioDebugGraph : MonoBehaviour {

	// Public Fields
	public FFTAnalyzer analyzer;
	public Vector2 heightRange;
	public AnimationCurve normalizer;
	public GameObject barPrefab;
	public Gradient colorGrade;

	// Internal Methods
	private List<GameObject> _managedObjects;
	private Vector2 _dimensions;

	void Start() {
		_dimensions = GetComponent<RectTransform>().sizeDelta;
		int quantity = analyzer.Spectrum.Length;
		_managedObjects = new List<GameObject>(quantity);
		for (int count = 0; count < quantity; count += 1) {
			GameObject instance = Instantiate(barPrefab, transform);
			_managedObjects.Add(instance);
			RectTransform rect = instance.GetComponent<RectTransform>();
			if (rect != null) {
				rect.sizeDelta = new Vector2(_dimensions.x / quantity, heightRange.x);
			}
		}
	}

	void Update() {
		for (int index = 0; index < _managedObjects.Count; index += 1) {
			float value = Mathf.Clamp01(analyzer.Spectrum[index] * normalizer.Evaluate((float)index / (float)analyzer.Spectrum.Length)); 

			// Set Bar Height
			RectTransform rect = _managedObjects[index].GetComponent<RectTransform>();
			if (rect != null) {
				rect.sizeDelta = new Vector2(rect.sizeDelta.x, Mathf.Lerp(heightRange.x, heightRange.y, value));
			}

			// Set Bar Color
			Image image = _managedObjects[index].GetComponent<Image>();
			if (image != null) {
				image.color = colorGrade.Evaluate(value);
			}
		}
	}
}
