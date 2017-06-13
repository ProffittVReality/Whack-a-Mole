using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;

public class GUI_Handler : MonoBehaviour {

	public GameObject menu; //Assign canvas to this in inspector, make sure script is on EventHandler
	public KeyCode hideShow; //Set to the key you want to press to open and close the GUI (also activates sword)
	public string calibrationFileName; // export calibration data file
	public string dataFileName; // export trial data file

	private bool isShowing;
	public InputField raName, participantNo, expNo, age, other;
	public Dropdown sex, vrExp, hand;
	public Text sexLabel, expLabel, handLabel;

	public GameObject rightSword;
	public GameObject leftSword;

	string theDate;
	string theTime;

	public bool isVisible {
		get {
			return menu.activeSelf;
		}
	}

	// Use this for initialization
	void Start () {
		if (calibrationFileName.Equals (""))
			calibrationFileName = "calibrationDefault";
		if (dataFileName.Equals (""))
			dataFileName = "dataDefault";

		rightSword.SetActive (false);
		leftSword.SetActive (false);

		//DateTime now = DateTime.Now;
		theTime = DateTime.Now.ToString ("hh:mm:ss");
		theDate = DateTime.Now.ToString ("d");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (hideShow)) {
			isShowing = !isShowing;
			menu.SetActive (isShowing);

			ActivateSword ();
		}
	}


	//TODO how to allow only once
	//	activeBalloon.hasBeenAttempted, set to true on first attempt
	public void exportCalibrationData(List<string> data) {
		string path = @"Assets\Data\" + calibrationFileName + ".csv";
		string delimiter = ",";
		if (!File.Exists(path)) {
			string header = "TimeStarted,ParticipantNumber,RAName,Age,Sex,Handedness,VRExperience,Trial,BalloonTimeSeconds,BalloonPopped,AmountCenter,HitInTime\n";
			File.WriteAllText (path, header);
		}
		string dataText = "";
		foreach (string item in data) {
			dataText += delimiter + item;
		}
		string fixedData = theDate + theTime + "," + participantNo.text + "," + raName.text + "," + age.text + "," + sexLabel.text + "," + handLabel.text + "," + expLabel.text;
		string appendText = fixedData + dataText + "\n";

		File.AppendAllText (path, appendText);
	}

	public void exportTrialData(List<string> data) {
		string path = @"Assets\Data\" + dataFileName + ".csv";
		string delimiter = ",";
		if (!File.Exists(path)) {
			string header = "TimeStarted,ParticipantNumber,RAName,Age,Sex,Handedness,VRExperience,Trial,BalloonTimeSeconds,NumberPracticed,BalloonPopped,AmountCenter,HitInTime,DriftType,DriftAmountDegrees\n";
			File.WriteAllText (path, header);
		}
		string dataText = "";
		foreach (string item in data) {
			dataText += delimiter + item;
		}
		string fixedData = theDate + theTime + "," + participantNo.text + "," + raName.text + "," + age.text + "," + sexLabel.text + "," + handLabel.text + "," + expLabel.text;
		string appendText = fixedData + dataText + "\n";

		File.AppendAllText (path, appendText);
	}

	void ActivateSword() {
		if (handLabel.text == "Left") {
			leftSword.SetActive (true);
		} else if (handLabel.text == "Right") {
			rightSword.SetActive (true);
		}
	}
}
