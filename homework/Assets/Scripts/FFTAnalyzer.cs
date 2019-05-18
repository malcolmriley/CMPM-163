using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class FFTAnalyzer : MonoBehaviour {

	// Public Fields
	public FFTWindow windowType;
	public int sampleSize;
	public float[] Spectrum { get; private set; }

	// Internal Methods
	private AudioListener _listener;

	public void Start() {
		_listener = GetComponent<AudioListener>();
		Spectrum = new float[Mathf.ClosestPowerOfTwo(sampleSize)];
	}

	public void Update() {
		AudioListener.GetSpectrumData(Spectrum, 0, windowType);
	}
}