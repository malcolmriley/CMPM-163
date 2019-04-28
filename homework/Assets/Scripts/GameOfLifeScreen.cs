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

	// Internal References
	private Texture2D _first;
	private Texture2D _second;
	private RenderTexture _renderTexture;
	private int _generation;
	private float _progress = 1.0F;
	private int _seed = 25;
	private float _speed = 0.05F;

	public override void OnInteract(TerminalManager manager, TerminalInput interaction) {
		switch (interaction) {
			case TerminalInput.UP:
				_speed = Mathf.Clamp01(_speed + 0.05F);
				break;
			case TerminalInput.DOWN:
				_speed = Mathf.Clamp01(_speed - 0.05F);
				break;
			case TerminalInput.LEFT:
				_seed = Mathf.Clamp(_seed - 1, 2, 30);
				break;
			case TerminalInput.RIGHT:
				_seed = Mathf.Clamp(_seed + 1, 2, 30);
				break;
			case TerminalInput.BACK:
				break;
			case TerminalInput.SELECT:
				ResetTextures();
				break;
			case TerminalInput.ANY:
				break;
		}
	}

	public override void OnScreenExit(TerminalManager manager) {
		ReleaseTextures();
	}

	public override void OnScreenLoad(TerminalManager manager) {
		ResetTextures();
	}

	public override void OnScreenUpdate(TerminalManager manager) {
		if (SpawnGeneration()) {
			Texture2D inputTexture = (_generation % 2 == 0) ? _first : _second;
			Texture2D outputTexture = (_generation % 2 == 0) ? _second : _first;

			Graphics.Blit(inputTexture, _renderTexture, automata);
			Graphics.CopyTexture(_renderTexture, outputTexture);

			gameRegion.material.SetTexture("_OverlayTexture", outputTexture);
			_generation += 1;
		}
		statsText.text = string.Format("Ratio: 1/{0} | Generation: {1} | Speed: {2:F}", _seed, _generation, _speed * 5);
	}

	// Internal Methods
	private bool SpawnGeneration() {
		_progress += _speed;
		if (_progress >= 1.0F) {
			_progress = 0.0F;
			return true;
		}
		return false;
	}

	private void ResetTextures() {
		_progress = 1.0F;
		_generation = 0;
		ReleaseTextures();
		GenerateTextures();
	}

	private void ReleaseTextures() {
		Destroy(_first);
		Destroy(_second);
		Destroy(_renderTexture);
	}

	private void GenerateTextures() {
		_first = GenerateTexture(true);
		_second = GenerateTexture(false);
		_renderTexture = new RenderTexture(_first.width, _first.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear) { filterMode = FilterMode.Point };
		_renderTexture.Create();
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
		return (Random.Range(0, _seed) == 0) ? liveColor : deadColor;
	}

	private Vector2 GetScaledSize() {
		return gameRegion.gameObject.GetComponent<RectTransform>().sizeDelta * 0.25F;
	}
}
