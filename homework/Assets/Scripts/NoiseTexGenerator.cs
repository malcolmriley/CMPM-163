using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTexGenerator : MonoBehaviour {

	// Public Fields
	public List<Material> managedMaterials;
	public Vector2Int dimensions;
	[Min(1.0F)]
	public float scale;

	// Internal Fields
	private const string TEXTURE_NAME = "_AlphaTex";

	void Start() {
		foreach (Material iteratedMaterial in managedMaterials) {
			iteratedMaterial.SetTexture(TEXTURE_NAME, GenerateNoiseTexture());
		}
	}

	// Internal Methods
	private Texture2D GenerateNoiseTexture() {
		Texture2D instance = new Texture2D(dimensions.x, dimensions.y);
		Color[] pixelData = new Color[dimensions.x * dimensions.y];
		for (int xPos = 0; xPos < dimensions.x; xPos += 1) {
			for (int yPos = 0; yPos < dimensions.y; yPos += 1) {
				float luma = Mathf.PerlinNoise(scale * (float)xPos / (float)dimensions.x, scale * (float)yPos / (float)dimensions.y);
				pixelData[yPos * dimensions.x + xPos] = new Color(luma, luma, luma);
			}
		}
		instance.SetPixels(pixelData);
		instance.Apply();
		instance.filterMode = FilterMode.Bilinear;
		return instance;
	}
}
