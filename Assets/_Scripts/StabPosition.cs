using UnityEngine;
using System.Collections;

public class StabPosition : MonoBehaviour {

	[HideInInspector]
	public Vector3 entryPosition;
	public GameObject swordTipL;
	public GameObject swordTipR;

	void OnTriggerEnter() {
		if (swordTipL.activeInHierarchy)
			entryPosition = swordTipL.transform.position;
		else if (swordTipR.activeInHierarchy)
			entryPosition = swordTipR.transform.position;
	}
}
