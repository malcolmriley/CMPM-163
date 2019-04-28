using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuItemController : MonoBehaviour {

	// Public References
	public GameObject nextScreen;
	public Image fileIcon;
	public Text filename;
	public Text filetype;

	public void SetIcon(Sprite newIcon) {
		fileIcon.sprite = newIcon;
	}

	public void SetFilename(string newName) {
		filename.text = newName;
	}

	public void SetFiletype(string newType) {
		filetype.text = newType;
	}

	public void SetSelected(bool value) {
		GetComponent<Image>().enabled = value;
	}
}
