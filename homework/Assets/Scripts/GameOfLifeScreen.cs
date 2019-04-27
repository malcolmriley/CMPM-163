using System.Collections;
using System.Collections.Generic;
using GameTerminal;
using UnityEngine;
using UnityEngine.UI;

public class GameOfLifeScreen : TerminalBehavior {

	// Public References
	public Text statsText;
	public Image gameRegion;
	public Material automata;
	[Range(0.001F, 1.0F)]
	public float speed = 0.02F;

	// Internal References
	private Texture2D _first;
	private Texture2D _second;
	private RenderTexture _renderTexture;
	private int _generation;
	private float _progress = 1.0F;

	public override void OnInteract(TerminalManager manager, TerminalInput interaction) {

	}

	public override void OnScreenExit(TerminalManager manager) {
		Destroy(_first);
		Destroy(_second);
		Destroy(_renderTexture);
	}

	public override void OnScreenLoad(TerminalManager manager) {
		_first = GenerateTexture(true);
		_second = GenerateTexture(true);
		_renderTexture = new RenderTexture(_first.width, _first.height, 0, RenderTextureFormat.ARGB32);
	}

	public override void OnScreenUpdate(TerminalManager manager) {
		if (SpawnGeneration()) {
			Texture2D inputTexture = (_generation % 2 == 0) ? _first : _second;
			Texture2D outputTexture = (_generation % 2 == 0) ? _second : _first;

			Graphics.Blit(inputTexture, _renderTexture, automata);
			Graphics.CopyTexture(inputTexture, outputTexture);

			gameRegion.material.SetTexture("_OverlayTexture", outputTexture);
			statsText.text = string.Format("Generation: {0}", _generation);
			_generation += 1;
		}
	}

	// Internal Methods
	private bool SpawnGeneration() {
		_progress += speed;
		if (_progress >= 1.0F) {
			_progress = 0.0F;
			return true;
		}
		return false;
	}

	private Texture2D GenerateTexture(bool initialize) {
		Vector2 size = GetScaledSize();
		int width = Mathf.FloorToInt(size.x);
		int height = Mathf.FloorToInt(size.y);
		Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false) {
			filterMode = FilterMode.Point
		};
		if (initialize) {
			for (int xPos = 0; xPos < width; xPos += 1) {
				for (int yPos = 0; yPos < height; yPos += 1) {
					texture.SetPixel(xPos, yPos, GeneratePixel(xPos, yPos));
				}
			}
			texture.Apply();
		}
		return texture;
	}

	private Color GeneratePixel(int xPos, int yPos) {
		Color liveColor = Color.white;
		Color deadColor = Color.black;
		return (Random.Range(0, 2) == 0) ? liveColor : deadColor;
	}

	private Vector2 GetScaledSize() {
		return gameRegion.gameObject.GetComponent<RectTransform>().sizeDelta * 0.5F;
	}
}
