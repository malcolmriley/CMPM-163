using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerminalScreen", menuName = "DataContainers/TerminalScreen")]
public class TerminalScreen : ScriptableObject {
	public TextAsset textAsset;
	[Range(0, 480)]
	public int screenWidth = 400;
	[Range(0, 320)]
	public int screenHeight = 320;
}
