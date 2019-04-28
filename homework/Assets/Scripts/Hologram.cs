using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Hologram", menuName = "DataContainers/Hologram")]
public class Hologram : ScriptableObject {
	// Public References
	public GameObject projection;
	public Sprite screenSprite;
	public string itemName;
	public string itemType;
}
