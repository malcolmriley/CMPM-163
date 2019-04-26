using System;
using System.Collections.Generic;
using UnityEngine;
using GameTerminal;
using UnityEngine.Events;

namespace GameTerminal {
	public enum TerminalInput {
		UP,
		DOWN,
		LEFT,
		RIGHT,
		BACK,
		SELECT,
		ANY,
	}

	public interface ITerminalBehavior {
		void OnScreenLoad(TerminalManager manager);
		void OnScreenExit(TerminalManager manager);
		void OnScreenUpdate(TerminalManager manager);
		void OnInteract(TerminalManager manager, TerminalInput interaction);
	}

	public abstract class TerminalBehavior : MonoBehaviour, ITerminalBehavior {
		public abstract void OnScreenLoad(TerminalManager manager);
		public abstract void OnScreenExit(TerminalManager manager);
		public abstract void OnScreenUpdate(TerminalManager manager);
		public abstract void OnInteract(TerminalManager manager, TerminalInput interaction);
	}

	[Serializable]
	public class TerminalInteraction : UnityEvent<TerminalManager, TerminalInput> { }

	[Serializable]
	public class TerminalEvent : UnityEvent<TerminalManager> { }
}

public class TerminalManager : MonoBehaviour {

	// Public Fields
	public Canvas canvas;
	public GameObject bootScreen;
	public TerminalInteraction interaction;
	public TerminalEvent onStart;
	public TerminalEvent onBoot;
	public TerminalEvent onUpdate;
	public TerminalEvent onExit;

	// Internal Fields
	private Dictionary<KeyCode, TerminalInput> _inputMap = new Dictionary<KeyCode, TerminalInput> {
		[KeyCode.Return] = TerminalInput.SELECT,
		[KeyCode.W] = TerminalInput.UP,
		[KeyCode.S] = TerminalInput.DOWN,
		[KeyCode.A] = TerminalInput.LEFT,
		[KeyCode.D] = TerminalInput.RIGHT,
		[KeyCode.Space] = TerminalInput.BACK
	};
	private GameObject _currentScreen;
	private GameObject _previousScreen;

	void Start() {
		onStart?.Invoke(this);
		Boot();
	}

	void Update() {
		HandleInteractions();
		onUpdate?.Invoke(this);
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnScreenUpdate(this);
	}

	public void Boot() {
		onBoot?.Invoke(this);
		if (!(bootScreen is null)) {
			_currentScreen = Instantiate(bootScreen, canvas.transform);
			_currentScreen.GetComponent<TerminalBehavior>()?.OnScreenLoad(this);
		}
	}

	public void Exit() {
		onExit?.Invoke(this);
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnScreenExit(this);
	}

	public void SetScreen(GameObject passedObject, bool destroyPrevious) {
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnScreenExit(this);
		passedObject?.GetComponent<TerminalBehavior>()?.OnScreenLoad(this);
		if (destroyPrevious) {
			Destroy(_currentScreen);
			Destroy(_previousScreen);
		}
		else {
			_previousScreen = _currentScreen;
		}
		_currentScreen = passedObject;
	}

	public Canvas GetCanvas() {
		return canvas;
	}

	// Internal Methods
	private void HandleInteractions() {
		if (Input.anyKeyDown) {
			AttemptInteraction(TerminalInput.ANY);
		}
		else foreach (KeyValuePair<KeyCode, TerminalInput> entry in _inputMap) {
			if (Input.GetKeyDown(entry.Key)) {
				AttemptInteraction(entry.Value);
				return;
			}
		}
	}

	private void AttemptInteraction(TerminalInput input) {
		interaction?.Invoke(this, input);
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnInteract(this, input);
	}
}
