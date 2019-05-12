using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraVFX : MonoBehaviour {

	public Material vfxMaterial;

	// Local References
	private Camera _camera;

	public void Start() {
		_camera = GetComponent<Camera>();
	}

	public void Update() {
		
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, destination, vfxMaterial);
	}
}
