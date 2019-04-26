using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSpawner : MonoBehaviour {

	// Public Fields
	public List<GameObject> objects;

	// Internal Fields
	private GameObject _currentObject;
	private int _index;

	public void Start() {
		Select(0);
	}

	public void Select(int index) {
		_index = index % (objects.Count - 1);
		GameObject prototype = objects[index];
		if (!(prototype is null)) {
			if (!(_currentObject is null)) {
				Destroy(_currentObject);
			}
			_currentObject = Instantiate(prototype, transform);
		}
	}

	public void Next() {
		Select(_index + 1);
	}

	public void Previous() {
		Select(_index - 1);
	}
}
