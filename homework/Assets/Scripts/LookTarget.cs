using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class LookTarget : MonoBehaviour {
	// Public Fields
	public Transform cameraMoveTarget;
	public Sprite interactOverride;
	public UnityEvent onLookAt;
	public bool isCurrentTarget;
}
