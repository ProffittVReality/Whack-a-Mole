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
	[HideInInspector]
	public float collisionTime;
	public Collider balloonCollider;
	[HideInInspector]
	public Vector3 entryPosition;
	[HideInInspector]
	public Vector3 startingPosition;
	[HideInInspector]
	public Quaternion startingRotation;
	public Transform balloonTransform;

	void Start() {
		startingPosition = balloonTransform.localPosition;
		startingRotation = balloonTransform.localRotation;
	}
}
