using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTerminal;
using UnityEngine.UI;

public class HoloScreen : TerminalBehavior {

	// Public Fields
	public Hologram hologram;
	public Text renderName;
	public Text renderType;

	// Internal Fields
	private GameObject _renderInstance;

	public override void OnInteract(TerminalManager manager, TerminalInput interaction) {
		switch (interaction) {
			case TerminalInput.UP:
				break;
			case TerminalInput.DOWN:
				break;
			case TerminalInput.LEFT:
				break;
			case TerminalInput.RIGHT:
				break;
			case TerminalInput.BACK:
				manager.ReturnToPrevious(true);
				break;
			case TerminalInput.SELECT:
				break;
			case TerminalInput.ANY:
				break;
		}
	}

	public override void OnScreenExit(TerminalManager manager) {
		Destroy(_renderInstance);
	}

	public override void OnScreenLoad(TerminalManager manager) {
		_renderInstance = Instantiate(hologram.projection, manager.projectorOutput);
		renderName.text = hologram.itemName;
		renderType.text = string.Format("Render Type: {0}", hologram.itemType);
	}

	public override void OnScreenUpdate(TerminalManager manager) {

	}
}

[CreateAssetMenu(fileName = "Hologram", menuName = "DataContainers/Hologram")]
public class Hologram : ScriptableObject {
	// Public References
	public GameObject projection;
	public string itemName;
	public string itemType;
}
