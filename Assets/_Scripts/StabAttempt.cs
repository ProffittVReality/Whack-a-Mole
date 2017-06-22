using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StabAttempt : MonoBehaviour {

	public TrialManagerScript trialManager;
	public BalloonSpawn balloonSpawn;
	public StabPosition stabPosition;

	public GUI_Handler guiHandler;
	public DriftController driftController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider collider) {
		// while inside trigger, sword cannot pop balloon

		float time = Time.time;

		if (trialManager.calibrationRound > 0) {
			if (collider.tag == "Sword") {

				// in case of miss
				if (balloonSpawn.balloonActive && !balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().hasPopped) {
					StartCoroutine (DeleteOnMiss ());
				}

				if (balloonSpawn.balloonActive && !balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().popAttempted) {
					balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().popAttempted = true;

					// calculate distance from center of balloon
					Vector3 hitPoint;
					if (balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().hasPopped) {
						hitPoint = balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().entryPosition;
					} else {
						hitPoint = stabPosition.entryPosition;
					}
					Vector3 balloonCenter = balloonSpawn.activeBalloon.transform.position;

					Vector3 unrotatedHitPoint = stabPosition.unrotatedEntryPosition;

					bool noTranslation = driftController.controllerSide == 0f && driftController.controllerUp == 0f;
					bool noRotation = driftController.controllerRotSide == 0f && driftController.controllerRotUp == 0f;

					float distance = CalculateDistance (balloonCenter, hitPoint);
					float noDriftDistance = CalculateDistance (balloonCenter, unrotatedHitPoint);
					if (noTranslation && noRotation)
						noDriftDistance = distance;

					// would they have hit the balloon? 
					float popTime = balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().popTime;

					bool inTime = !(time > popTime);

					// did balloon pop? 
					bool popSuccess = balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().hasPopped;

					balloonSpawn.activeBalloon.GetComponent<BalloonScript> ().dataTaken = true;

					if (trialManager.calibrationMode) {
						// export calibration data
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
						// export trial data
						string trialNumber = "Data" + trialManager.trialNumber.ToString () + "." + balloonSpawn.subTrial.ToString ();
						string balloonTimeSeconds = trialManager.balloonLifetime.ToString ();
						string numberPracticed = trialManager.totalCalibrationRounds.ToString ();
						string balloonPopped = popSuccess ? "Y" : "N";
						string amountCenter = distance.ToString ();
						string noDriftAmountCenter;
						if (driftController.controllerRotate || driftController.controllerTranslate) {
							noDriftAmountCenter = noDriftDistance.ToString ();
						} else {
							noDriftAmountCenter = "No Controller Drift";
						}
						string hitInTime = inTime ? "Y" : "N";
						string driftType = guiHandler.driftLabel.text;
						List<string> driftAmount = driftController.GetDriftAmount();
						List<string> exportList = new List<string> {
							trialNumber,
							balloonTimeSeconds,
							numberPracticed,
							balloonPopped,
							amountCenter,
							noDriftAmountCenter,
							hitInTime,
							driftType,
						};
						exportList.AddRange (driftAmount);
						guiHandler.exportTrialData (exportList);
					}
				} 
			}
		}
	}

	IEnumerator DeleteOnMiss() {
		// when user misses balloon, it disappears after 0.5s
		GameObject activeBalloon = balloonSpawn.activeBalloon;
		yield return new WaitForSeconds (0.5f);
		activeBalloon.SetActive (false);
	}

	float CalculateDistance(Vector3 balloonPosition, Vector3 entryPosition) {
		float xDistance = balloonPosition.x - entryPosition.x;
		float yDistance = balloonPosition.y - entryPosition.y;

		if (xDistance < 0)
			xDistance = -xDistance;
		if (yDistance < 0)
			yDistance = -yDistance;

		return Mathf.Pow ((Mathf.Pow (yDistance, 0.5f) + Mathf.Pow (xDistance, 0.5f)), 2f);
	}
}
