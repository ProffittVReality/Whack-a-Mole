using UnityEngine;
using System.Collections;

public class BalloonScript : MonoBehaviour {

	[HideInInspector]
	public bool hasPopped;
	[HideInInspector]
	public bool popAttempted;

	[HideInInspector]
	public bool dataTaken = false;

	[HideInInspector]
	public float popTime;
	public Collider balloonCollider;

	public Vector3 entryPosition;
}
