using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameTerminal;

public class TerminalManager : MonoBehaviour {

	// Public Fields
	public Text textElement;
	public TerminalScreen loadScreen;

	// Internal Fields
	private TerminalScreen _screen;
	private TerminalScreen _previous;
	private Dictionary<KeyCode, TerminalInteraction> _inputMap = new Dictionary<KeyCode, TerminalInteraction>();

	void Start() {
		_inputMap[KeyCode.Return] = TerminalInteraction.SELECT;
		_inputMap[KeyCode.W] = TerminalInteraction.UP;
		_inputMap[KeyCode.S] = TerminalInteraction.DOWN;
		_inputMap[KeyCode.A] = TerminalInteraction.LEFT;
		_inputMap[KeyCode.D] = TerminalInteraction.RIGHT;
		_inputMap[KeyCode.Space] = TerminalInteraction.BACK;
		Boot();
	}

	void Update() {
		_screen?.OnScreenUpdate(this);
		HandleInteractions(_screen);
	}

	public void SwitchToScreen(TerminalScreen newScreen, bool canReturn) {
		_screen?.OnScreenExit(this);
		SetParametersfrom(newScreen);
		newScreen.OnScreenLoad(this);
		_previous = canReturn ? _screen : null;
		_screen = newScreen;
	}

	public void SetScreenText(string passedText) {
		textElement.text = passedText;
	}

	public void ReturnToPrevious() {
		if (!(_previous is null)) {
			SwitchToScreen(_previous, false);
		}
	}

	public void Boot() {
		SwitchToScreen(loadScreen, false);
	}

	public Canvas GetCanvas() {
		return textElement.canvas;
	}

	// Internal Methods
	private void HandleInteractions(TerminalScreen behavior) {
		if (Input.anyKeyDown) {
			behavior.OnInteract(this, TerminalInteraction.ANY);
		}
		else foreach (KeyValuePair<KeyCode, TerminalInteraction> entry in _inputMap) {
			if (Input.GetKeyDown(entry.Key)) {
				behavior.OnInteract(this, entry.Value);
				return;
			}
		}
	}

	private void SetParametersfrom(TerminalScreen terminalScreen) {
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, terminalScreen.screenWidth);
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, terminalScreen.screenHeight);
		textElement.alignment = terminalScreen.anchor;
	}
}
