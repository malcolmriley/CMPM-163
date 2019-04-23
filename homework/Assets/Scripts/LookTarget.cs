using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class LookTarget : MonoBehaviour {
	// Public Fields
	public Transform cameraMoveTarget;
	public Sprite interactionSprite;
}
