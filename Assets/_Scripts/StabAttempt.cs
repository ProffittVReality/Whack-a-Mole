using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StabAttempt : MonoBehaviour {

	public TrialManagerScript trialManager;
	public BalloonSpawn balloonSpawn;
	public StabPosition stabPosition;

	public GUI_Handler guiHandler;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider collider) {
		// while inside trigger, sword cannot pop balloon

		//Debug.Log (string.Format ("collider.tag == Sword: {0}", collider.tag == "Sword"));
		if (trialManager.calibrationRound > 0) {
			if (collider.tag == "Sword") {

				// in case of miss
				if (!balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().hasPopped) {
					StartCoroutine (DeleteOnMiss ());
				}

				if (balloonSpawn.balloonActive && !balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().popAttempted) {
					balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().popAttempted = true;

					// calculate distance from center of balloon TODO test this
					Vector3 balloonCenter = balloonSpawn.activeBalloon.transform.position;
					Vector3 hitPoint = stabPosition.entryPosition;
					float xDistance = hitPoint.x - balloonCenter.x;
					float yDistance = hitPoint.y - balloonCenter.y;

					if (xDistance < 0)
						xDistance = -xDistance;
					if (yDistance < 0)
						yDistance = -yDistance;
					
					float distance = Mathf.Pow ((Mathf.Pow (yDistance, 0.5f) + Mathf.Pow (xDistance, 0.5f)), 2f);

					// would they have hit the balloon? 
					float popTime = balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().popTime;
					float time = Time.time;
					bool inTime = !(time > popTime);

					// did balloon pop? TODO this does not work
					bool popSuccess = balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().hasPopped;

					if (trialManager.calibrationMode) {
						string trialNumber = "Prac" + trialManager.calibrationRound.ToString () + "." + balloonSpawn.subTrial.ToString ();
						string balloonTimeSeconds = trialManager.balloonLifetime.ToString (); 
						string balloonPopped = popSuccess ? "Y" : "N";
						string amountCenter = distance.ToString ();
						string hitInTime = inTime ? "Y" : "N";
						guiHandler.exportCalibrationData (new List<string> {
							trialNumber,
							balloonTimeSeconds,
							balloonPopped,
							amountCenter,
							hitInTime
						});

					} else if (trialManager.trialMode) {
						string trialNumber = trialManager.trialNumber.ToString () + "." + balloonSpawn.subTrial.ToString ();
						string balloonTimeSeconds = trialManager.balloonLifetime.ToString ();
						string numberPracticed = trialManager.totalCalibrationRounds.ToString ();
						string balloonPopped = popSuccess ? "Y" : "N";
						string amountCenter = distance.ToString ();
						string hitInTime = inTime ? "Y" : "N";
						string driftType = "TODO";
						string driftAmountDegrees = "TODO";
						guiHandler.exportTrialData (new List<string> {
							trialNumber,
							balloonTimeSeconds,
							numberPracticed,
							balloonPopped,
							amountCenter,
							hitInTime,
							driftType,
							driftAmountDegrees
						});
					}
				} 
			}
		}
	}

	IEnumerator DeleteOnMiss() {
		GameObject activeBalloon = balloonSpawn.activeBalloon;
		yield return new WaitForSeconds (0.5f);
		activeBalloon.SetActive (false);
	}
}
