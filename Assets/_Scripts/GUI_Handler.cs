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
	public Dropdown sex, vrExp, hand, drift;
	public Text sexLabel, expLabel, handLabel, driftLabel;

	public GameObject rightSword;
	public GameObject leftSword;

	string theDate;

	public bool isVisible {
		get {
			return menu.activeSelf;
		}
	}

	public DriftController driftController;

	// Use this for initialization
	void Start () {
		if (calibrationFileName.Equals (""))
			calibrationFileName = "calibrationDefault";
		if (dataFileName.Equals (""))
			dataFileName = "dataDefault";

		rightSword.SetActive (false);
		leftSword.SetActive (false);

		theDate = DateTime.Now.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (hideShow)) {
			isShowing = !isShowing;
			menu.SetActive (isShowing);

			ActivateSword ();

			// set bools for rotation types
			if (driftLabel.text == "Controller Translation Only") {
				driftController.controllerRotate = false;
				driftController.controllerTranslate = true;
				driftController.headsetRotate = false;
			}
			// Controller Rotation only
			if (driftLabel.text == "Controller Rotation Only") {
				driftController.controllerRotate = true;
				driftController.controllerTranslate = false;
				driftController.headsetRotate = false;
			}
			// Headset Rotation only
			if (driftLabel.text == "Headset Rotation Only") {
				driftController.controllerRotate = false;
				driftController.controllerTranslate = false;
				driftController.headsetRotate = true;
			}
			// Controller Rotation and Headset Rotation
			if (driftLabel.text == "Headset Rotation and Controller Rotation") {
				driftController.controllerRotate = true;
				driftController.controllerTranslate = false;
				driftController.headsetRotate = true;
			}
			// Controller Translation and Headset Rotation
			if (driftLabel.text == "Headset Rotation and Controller Translation") {
				driftController.controllerRotate = false;
				driftController.controllerTranslate = true;
				driftController.headsetRotate = true;
			}
			// Controller Translation and Controller Rotation
			if (driftLabel.text == "Controller Rotation and Controller Translation") {
				driftController.controllerRotate = true;
				driftController.controllerTranslate = true;
				driftController.headsetRotate = false;
			}
		}
	}
		
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
		string fixedData = theDate + "," + participantNo.text + "," + raName.text + "," + age.text + "," + sexLabel.text + "," + handLabel.text + "," + expLabel.text;
		string appendText = fixedData + dataText + "\n";

		File.AppendAllText (path, appendText);
	}

	public void exportTrialData(List<string> data) {
		string path = @"Assets\Data\" + dataFileName + ".csv";
		string delimiter = ",";
		if (!File.Exists(path)) {
			string header = "TimeStarted,ParticipantNumber,RAName,Age,Sex,Handedness,VRExperience,Trial,BalloonTimeSeconds,NumberPracticed,BalloonPopped,AmountCenter,NoDriftAmountCenter,HitInTime,DriftType,ControllerTranslationUp,ControllerTranslationForward,ControllerTranslationSide,ControllerRotationUp,ControllerRotationSide,HeadsetRotation\n";
			File.WriteAllText (path, header);
		}
		string dataText = "";
		foreach (string item in data) {
			dataText += delimiter + item;
		}
		string fixedData = theDate + "," + participantNo.text + "," + raName.text + "," + age.text + "," + sexLabel.text + "," + handLabel.text + "," + expLabel.text;
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
