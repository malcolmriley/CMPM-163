using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MenuItem", menuName = "DataContainers/MenuItem")]
public class MenuItem : ScriptableObject {

	// Public References
	public GameObject screenPrefab;
	public Sprite itemIcon;
	public string itemName;
	public string itemType;
}
