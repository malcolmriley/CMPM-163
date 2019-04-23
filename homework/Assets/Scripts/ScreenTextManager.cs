using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTextManager : MonoBehaviour {

	// Public Fields
	public Text textElement;
	public TerminalScreen screen;

	void Start() {
		SetParameterSfrom(screen);
	}

	void Update() {
		
	}

	private void SetParameterSfrom(TerminalScreen terminalScreen) {
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, terminalScreen.screenWidth);
		textElement.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, terminalScreen.screenHeight);
		textElement.text = terminalScreen.textAsset.text;
	}
}
