using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Valve.VR;

[System.Serializable]
public class CalibrationSettings
{
	public float passNum;

	public float initialLifetime;
	public float passRoundDecrement;
	public float failRoundIncrement;
}

public class TrialManagerScript : MonoBehaviour {

	// for on screen text display
	public UnityEngine.UI.Text trialText;
	public GameObject nextTrialAlert;

	public BalloonSpawn spawnController;

	public CalibrationSettings calibrationSettings;

	[HideInInspector]
	public int calibrationRound = 0;
	[HideInInspector]
	public bool calibrationMode = true;

	[HideInInspector]
	public int trialNumber = 0;
	[HideInInspector]
	public bool trialMode = false;
	public int numTrials;

	[HideInInspector]
	public float balloonLifetime;
	[HideInInspector]
	public float popCount;

	[HideInInspector]
	public bool roundOver = true; // initialize to true to start first trial
	bool gotEight = false; // first 8 of calibration round is achieved

	public KeyCode nextTrial;

	[HideInInspector]
	public float totalCalibrationRounds; // for calibration data file

	// Use this for initialization
	void Start () {
		popCount = 0;
		balloonLifetime = calibrationSettings.initialLifetime;
		nextTrialAlert.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (calibrationMode) {
			if (calibrationRound == 0) {
				trialText.text = "Calibration Mode";
			} else {
				string roundText = calibrationRound.ToString ();
				trialText.text = "Calibration Mode: Round " + roundText;
			}

			CalibrationPhase ();
		}

		if (trialMode && (trialNumber <= numTrials)) {
			if (trialNumber == 0) {
				trialText.text = "Begin Trials";
			} else {
				string trialNumText = trialNumber.ToString ();
				trialText.text = "Trial Number " + trialNumText;
			}

			ExperimentPhase ();
		}

		if (trialNumber == numTrials && roundOver) {
			trialText.text = "Experiment Over";
		}
			
	}

	void CalibrationPhase() {
		//Debug.Log ("roundOver: " + roundOver.ToString ());
		if (roundOver) {
			if (popCount >= calibrationSettings.passNum && gotEight) {
				trialMode = true;
				totalCalibrationRounds = calibrationRound;
				calibrationMode = false;
			}
			nextTrialAlert.SetActive (true);
			if (Input.GetKeyDown (nextTrial)) {
				if (popCount >= calibrationSettings.passNum-1 && popCount <= calibrationSettings.passNum+1) {
					gotEight = true;
					calibrationSettings.passRoundDecrement *= 0.5f;
					calibrationSettings.failRoundIncrement *= 0.5f;
				}

				nextTrialAlert.SetActive (false); // turn off indicator

				// set new balloon lifetime
				if (calibrationRound >= 1) { // make no adjustments before first trial
					Debug.Log ("popCount: " + popCount.ToString ());

					if (popCount > calibrationSettings.passNum+1) {
						balloonLifetime -= calibrationSettings.passRoundDecrement;
					} else if (popCount < calibrationSettings.passNum-1) {
						balloonLifetime += calibrationSettings.failRoundIncrement;
						gotEight = false;
					}
				}
				popCount = 0; 
				calibrationRound += 1;
				spawnController.StartTrial (); // set initial conditions for trial
				}
		}
		if (calibrationRound > 0) { // so a trial does not start without key press
			roundOver = spawnController.RunTrial (balloonLifetime);
		}
	}

	void ExperimentPhase() {
		if (roundOver && trialNumber != numTrials) {
			nextTrialAlert.SetActive (true);
			if (Input.GetKeyDown (nextTrial)) {
				nextTrialAlert.SetActive (false); // turn off indicator

				spawnController.StartTrial (); // set initial conditions for trial
				popCount = 0; 
				trialNumber += 1;
			}
		}
		roundOver = spawnController.RunTrial (balloonLifetime);
	}

}
