using System.Collections.Generic;
using UnityEngine;
using GameTerminal;

public class MenuScreen : TerminalBehavior {

	// Public Fields
	public RectTransform menuRegion;
	public GameObject menuItemPrefab;
	public List<MenuItem> menuItems;

	// Internal Fields
	private List<GameObject> menuInstances;
	private int _selected;
	private bool _loaded;

	public override void OnInteract(TerminalManager manager, TerminalInput interaction) {
		switch (interaction) {
			case TerminalInput.UP:
				Select(_selected - 1);
				break;
			case TerminalInput.DOWN:
				Select(_selected + 1);
				break;
			case TerminalInput.LEFT:
				break;
			case TerminalInput.RIGHT:
				break;
			case TerminalInput.BACK:
				break;
			case TerminalInput.SELECT:
				manager.SetScreen(GetSelected().nextScreen, false);
				break;
			case TerminalInput.ANY:
				break;
		}
	}

	public override void OnScreenExit(TerminalManager manager) {

	}

	public override void OnScreenLoad(TerminalManager manager) {
		if (!_loaded) {
			menuInstances = new List<GameObject>();
			float offset = 0;
			const float padding = 6.0F;
			foreach (MenuItem iteratedItem in menuItems) {
				GameObject newMenuInstance = Instantiate(menuItemPrefab, menuRegion);
				// newMenuInstance.transform.localPosition = new Vector3(0, offset, 0);
				ConfigureMenuItem(newMenuInstance, iteratedItem);
				menuInstances.Add(newMenuInstance);

				RectTransform newTransform = newMenuInstance.GetComponent<RectTransform>();
				if (!(newTransform is null)) {
					offset += newTransform.sizeDelta.y;
				}
				offset += padding;
			}
			_loaded = true;
			Select(0);
		}
	}

	public override void OnScreenUpdate(TerminalManager manager) {

	}

	// Internal Methods
	private void Select(int value) {
		GetSelected().SetSelected(false);
		_selected = (menuItems.Count + value) % menuItems.Count;
		GetSelected().SetSelected(true);
	}

	public MenuItemController GetSelected() {
		return menuInstances[_selected].GetComponent<MenuItemController>();
	}

	private void ConfigureMenuItem(GameObject instance, MenuItem template) {
		MenuItemController controller = instance.GetComponent<MenuItemController>();
		if (!(controller is null)) {
			controller.SetIcon(template.itemIcon);
			controller.SetFilename(template.itemName);
			controller.SetFiletype(template.itemType);
			controller.nextScreen = template.screenPrefab;
		}
	}
}
