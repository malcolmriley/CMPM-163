using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTerminal;
using UnityEngine.UI;
using System.Text;

public class TextScreen : TerminalBehavior {

	// Public Fields
	public TerminalText textObject;
	public Text textField;

	// Internal Objects
	private bool _loaded = false;
	private StringBuilder _builder = null;
	private float _loadProgress = 0.0F;
	private int _lineProgress = 0;
	private string[] _lines = null;
	protected bool _completed = false;

	public override void OnScreenLoad(TerminalManager manager) {
		ApplyProperties(textField);
		if (textObject.loadSpeed < 1.0F) {
			_builder = new StringBuilder();
			_lines = textObject.textAsset.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
			_builder.AppendLine(_lines[0]);
		}
		else {
			textField.text = textObject.textAsset.text;
			_completed = true;
		}
		_loaded = true;
	}

	public override void OnScreenExit(TerminalManager manager) {
		_builder?.Clear();
	}

	public override void OnScreenUpdate(TerminalManager manager) {
		if (_loaded && !_completed) {
			_loadProgress += textObject.loadSpeed - Random.Range(0.7F * textObject.loadSpeed, 0.0F);
			if (_loadProgress >= 1.0F) {
				_lineProgress += 1;
				_loadProgress = 0.0F;
				if (_lineProgress < _lines.Length) {
					_builder.AppendLine(_lines[_lineProgress]);
				}
				else {
					_completed = true;
				}
				textField.text = _builder.ToString();
			}
		}
	}

	public override void OnInteract(TerminalManager manager, TerminalInput interaction) {

	}

	private void ApplyProperties(Text textElement) {
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textObject.screenWidth);
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textObject.screenHeight);
		textElement.alignment = textObject.anchor;
	}
}
