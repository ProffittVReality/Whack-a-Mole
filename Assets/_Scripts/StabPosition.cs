using UnityEngine;
using System.Collections;

public class StabPosition : MonoBehaviour {

	[HideInInspector]
	public Vector3 entryPosition;
	[HideInInspector]
	public Vector3 unrotatedEntryPosition;
	public GameObject swordTipL;
	public GameObject swordTipR;
	public GameObject unrotatedL;
	public GameObject unrotatedR;

	void OnTriggerEnter() {
		if (swordTipL.activeInHierarchy) {
			entryPosition = swordTipL.transform.position;
			unrotatedEntryPosition = unrotatedL.transform.position;
		}
		else if (swordTipR.activeInHierarchy) {
			entryPosition = swordTipR.transform.position;
			unrotatedEntryPosition = unrotatedR.transform.position;
		}
	}
}
