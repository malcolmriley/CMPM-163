using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSound : MonoBehaviour {

	// Public Fields
	public List<AudioClip> sounds;

	// Internal Fields
	private AudioSource _source;

	public void Start() {
		_source = GetComponent<AudioSource>();
	}

	public void PlayRandom() {
		int index = Random.Range(0, sounds.Count);
		_source.PlayOneShot(sounds[index]);
	}
}
