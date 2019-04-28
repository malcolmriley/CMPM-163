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
	public MeshRenderer screenMesh;
	public GameObject bootScreen;
	public TerminalInteraction interaction;
	public TerminalEvent onStart;
	public TerminalEvent onBoot;
	public TerminalEvent onUpdate;
	public TerminalEvent onExit;

	[Range(0.0F, 0.1F)]
	public float effectRate = 0.01F;

	// Internal Fields
	private Dictionary<KeyCode, TerminalInput> _inputMap = new Dictionary<KeyCode, TerminalInput> {
		[KeyCode.Return] = TerminalInput.SELECT,
		[KeyCode.W] = TerminalInput.UP,
		[KeyCode.S] = TerminalInput.DOWN,
		[KeyCode.A] = TerminalInput.LEFT,
		[KeyCode.D] = TerminalInput.RIGHT,
		[KeyCode.Space] = TerminalInput.BACK
	};
	private float _effectIntensity;
	private bool _booted;
	private GameObject _currentScreen;
	private GameObject _previousScreen;

	private const string SCAN_INTENSITY = "_ScanIntensity";
	private const string BLUR_INTENSITY = "_Sharpness";

	void Start() {
		onStart?.Invoke(this);
	}

	void Update() {
		HandleInteractions();
		onUpdate?.Invoke(this);
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnScreenUpdate(this);
		if (_booted) {
			_effectIntensity = Mathf.Clamp(_effectIntensity - effectRate, -3.0F, 1.0F);
			ApplyEffects(_effectIntensity);
		}
	}

	public void Boot() {
		if (!_booted) {
			onBoot?.Invoke(this);
			if (!(bootScreen is null)) {
				_currentScreen = Instantiate(bootScreen, canvas.transform);
				_currentScreen.GetComponent<TerminalBehavior>()?.OnScreenLoad(this);
			}
			ResetMonitor();
			ApplyEffects(_effectIntensity);
			_booted = true;
		}
	}

	public void ResetMonitor() {
		_effectIntensity = 1.0F;
	}

	public void Exit() {
		onExit?.Invoke(this);
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnScreenExit(this);
		ApplyEffects(0.0F);
	}

	public void ReturnToPrevious(bool destroyCurrent) {
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnScreenExit(this);
		if (!(_previousScreen is null)) {
			GameObject newScreen = _previousScreen;
			newScreen?.GetComponent<TerminalBehavior>()?.OnScreenLoad(this);
			if (destroyCurrent) {
				Destroy(_currentScreen);
			}
			else {
				_previousScreen = _currentScreen;
			}
			_currentScreen = newScreen;
		}
	}

	public void SetScreen(GameObject passedObject, bool destroyPrevious) {
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnScreenExit(this);
		GameObject newScreen = Instantiate(passedObject, canvas.transform);
		newScreen?.GetComponent<TerminalBehavior>()?.OnScreenLoad(this);
		if (destroyPrevious) {
			Destroy(_currentScreen);
			Destroy(_previousScreen);
		}
		else {
			_previousScreen = _currentScreen;
			_previousScreen.SetActive(false);
		}
		_currentScreen = newScreen;
		_currentScreen.SetActive(true);
	}

	public Canvas GetCanvas() {
		return canvas;
	}

	// Internal Methods
	private void HandleInteractions() {
		foreach (KeyValuePair<KeyCode, TerminalInput> entry in _inputMap) {
			if (Input.GetKeyDown(entry.Key)) {
				AttemptInteraction(entry.Value);
				return;
			}
		}
		if (Input.anyKeyDown) {
			AttemptInteraction(TerminalInput.ANY);
		}
	}

	private void AttemptInteraction(TerminalInput input) {
		interaction?.Invoke(this, input);
		_currentScreen?.GetComponent<TerminalBehavior>()?.OnInteract(this, input);
	}

	private void ApplyEffects(float intensity) {
		SetMaterialProperty(BLUR_INTENSITY, intensity);
		SetMaterialProperty(SCAN_INTENSITY, 0.08F * (intensity + 1));
	}

	private void SetMaterialProperty(string property, float value) {
		screenMesh?.material.SetFloat(property, value);
	}
}
