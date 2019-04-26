using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

	// Public Fields
	[Range(0.01F, 5.0F)]
	public float intensity = 2.0F;

	// Private fields
	private Vector3 _position;

	// Start is called before the first frame update
	void Start() {
		_position = transform.localPosition;
	}

	// Update is called once per frame
	void Update() {
		float offset = intensity * Mathf.Sin(Time.timeSinceLevelLoad);
		transform.localPosition = new Vector3(_position.x, _position.y + offset, _position.z);
	}
}
