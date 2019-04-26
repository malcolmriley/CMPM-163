using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameTerminal;

public class TerminalManager : MonoBehaviour {

	// Public Fields
	public Canvas canvas;
	public TerminalInteraction interaction;
	public TerminalEvent onStart;
	public TerminalEvent onLoad;
	public TerminalEvent onUpdate;
	public TerminalEvent onExit;

	// Internal Fields
	private Dictionary<KeyCode, TerminalInput> _inputMap = new Dictionary<KeyCode, TerminalInput>() {
		[KeyCode.Return] = TerminalInput.SELECT,
		[KeyCode.W] = TerminalInput.UP,
		[KeyCode.S] = TerminalInput.DOWN,
		[KeyCode.A] = TerminalInput.LEFT,
		[KeyCode.D] = TerminalInput.RIGHT,
		[KeyCode.Space] = TerminalInput.BACK
	};

	void Start() {
		onStart?.Invoke(this);
	}

	void Update() {
		HandleInteractions();
		onUpdate?.Invoke(this);
	}

	public void Load() {
		onLoad?.Invoke(this);
	}

	public void Exit() {
		onExit?.Invoke(this);
	}

	public Canvas GetCanvas() {
		return canvas;
	}

	// Internal Methods
	private void HandleInteractions() {
		if (Input.anyKeyDown) {
			interaction?.Invoke(this, TerminalInput.ANY);
		}
		else foreach (KeyValuePair<KeyCode, TerminalInput> entry in _inputMap) {
			if (Input.GetKeyDown(entry.Key)) {
				interaction?.Invoke(this, entry.Value);
				return;
			}
		}
	}
}
