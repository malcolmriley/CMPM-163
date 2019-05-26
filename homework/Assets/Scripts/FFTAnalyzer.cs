using UnityEngine;

public class FFTAnalyzer : MonoBehaviour {

	// Public Fields
	public FFTWindow windowType;
	public int sampleSize;
	public float[] Spectrum { get; private set; }

	// Internal Methods

	public void Start() {
		Spectrum = new float[Mathf.ClosestPowerOfTwo(sampleSize)];
	}

	public void Update() {
		AudioListener.GetSpectrumData(Spectrum, 0, windowType);
	}
}