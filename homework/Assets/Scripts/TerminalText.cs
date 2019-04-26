using UnityEngine;
using GameTerminal;
using System;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TerminalText", menuName = "DataContainers/TerminalText")]
public class TerminalText : ScriptableObject {
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
}