using UnityEngine;

public class LightController : MonoBehaviour {

	// Public Fields
	public Light managedLight;
	public MeshRenderer managedRenderer;

	// Internal Fields;
	private bool _enabled;
	private const string EMISSION = "_EmissionColor";
	private Color _emissionColor;

	public void Start() {
		_emissionColor = managedLight.color;
		SetLights(_enabled);
	}

	public void ToggleLights() {
		SetLights(!_enabled);
	}

	public void SetLights(bool activated) {
		_enabled = activated;
		managedLight.enabled = activated;
		if (activated) {
			managedRenderer.material.SetColor(EMISSION, _emissionColor);
		}
		else {
			managedRenderer.material.SetColor(EMISSION, Color.black);
		}
	}

	// Update is called once per frame
	void Update() {
		
	}
}
