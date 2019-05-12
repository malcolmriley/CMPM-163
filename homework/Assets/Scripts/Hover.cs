using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

	// Public Fields
	[Range(0.01F, 5.0F)]
	public float intensity = 2.0F;
	public bool randomize = true;

	// Private fields
	private Vector3 _position;
	private float _offset;

	// Start is called before the first frame update
	void Start() {
		_position = transform.localPosition;
		if (randomize) {
			_offset = Random.Range(0.0F, 1.0F) * 10.0F;
		}
	}

	// Update is called once per frame
	void Update() {
		float offset = intensity * Mathf.Sin(Time.timeSinceLevelLoad + _offset);
		transform.localPosition = new Vector3(_position.x, _position.y + offset, _position.z);
	}
}
