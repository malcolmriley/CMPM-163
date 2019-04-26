using UnityEngine;
using GameTerminal;
using System.Text;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TerminalText", menuName = "DataContainers/TerminalText")]
public class TerminalText : ScriptableObject, ITerminalBehavior {
	// Public Constants
	public const int WIDTH_DEFAULT = 400;
	public const int HEIGHT_DEFAULT = 320;

	// Public References
	public TextAnchor anchor = TextAnchor.UpperLeft;
	public TextAsset textAsset;
	[Range(0, 480)]
	public int screenWidth = WIDTH_DEFAULT;
	[Range(0, 320)]
	public int screenHeight = HEIGHT_DEFAULT;
	[Range(0.01F, 1.0F)]
	public float loadSpeed = 0.05F;

	// Internal Objects
	[NonSerialized]
	private bool _loaded = false;
	[NonSerialized]
	private StringBuilder _builder = null;
	[NonSerialized]
	private float _loadProgress = 0.0F;
	[NonSerialized]
	private int _lineProgress = 0;
	[NonSerialized]
	private string[] _lines = null;
	[NonSerialized]
	protected bool _completed = false;

	public void ApplyProperties(Text textElement) {
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth);
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, screenHeight);
		textElement.alignment = anchor;
	}

	public virtual void OnScreenLoad(TerminalManager manager) {
		if (loadSpeed < 1.0F) {
			_builder = new StringBuilder();
			_lines = textAsset.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
			_builder.AppendLine(_lines[0]);
		}
		else {
			_completed = true;
		}
		_loaded = true;
	}

	public virtual void OnScreenExit(TerminalManager manager) {
		_builder?.Clear();
	}

	public virtual void OnScreenUpdate(TerminalManager manager) {
		if (_loaded && !_completed) {
			_loadProgress += loadSpeed;
			if (_loadProgress >= 1.0F) {
				_lineProgress += 1;
				_loadProgress = 0.0F;
				if (_lineProgress < _lines.Length) {
					_builder.AppendLine(_lines[_lineProgress]);
				}
				else {
					_completed = true;
				}
			}
		}
	}

	public virtual void OnInteract(TerminalManager manager, TerminalInput interaction) {

	}
}

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

	[Serializable]
	public class TerminalInteraction : UnityEvent<TerminalManager, TerminalInput> { }

	[Serializable]
	public class TerminalEvent : UnityEvent<TerminalManager> { }
}