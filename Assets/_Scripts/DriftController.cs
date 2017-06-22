using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DriftController : MonoBehaviour {

	[System.Serializable]
	public class DriftSettings {
		public int controllerRotationNumber;
		public float controllerRotationInterval;

		public int controllerTransNumber;
		public float controllerTransInterval;

		public int roomRotationNumber;
		public float roomRotationInterval;
	}
	float[] controllerRotations;
	float[] controllerTranslations;
	float[] roomRotations;

	public GameObject cameraRig;
	public GameObject room;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject leftSword;
	public GameObject rightSword;

	float prevRoomRotation;
	float prevControllerUp;
	float prevControllerSide;
	float prevControllerForward;
	float prevControllerRotUp;
	float prevControllerRotSide;

	Quaternion initialLeftSwordRotation;
	Quaternion initialRightSwordRotation;
	Vector3 initialLeftSwordPosition;
	Vector3 initialRightSwordPosition;

	Quaternion initialRoomRotation;
	Vector3 initialRoomPosition;

	public BalloonSpawn balloonSpawn;

	public DriftSettings driftSettings;


	System.Random rnd;

	[HideInInspector]
	public float controllerUp, controllerForward, controllerSide, controllerRotUp, controllerRotSide, headsetRot;
	[HideInInspector]
	public bool controllerRotate, controllerTranslate, headsetRotate;

	// Use this for initialization
	void Start () {
		rnd = new System.Random ();

		initialRoomRotation = room.transform.rotation;
		initialRoomPosition = room.transform.position;
		initialLeftSwordRotation = leftSword.transform.localRotation;
		initialRightSwordRotation = rightSword.transform.localRotation;

		roomRotations = new float[driftSettings.roomRotationNumber];
		controllerRotations = new float[driftSettings.controllerRotationNumber];
		controllerTranslations = new float[driftSettings.controllerTransNumber];

		// Generate controller rotations, controller translations, and room rotations
		float cRotNum = 0f;
		for (int i = 0; i < driftSettings.controllerRotationNumber; i++) { 
			controllerRotations[i] = cRotNum;
			cRotNum += driftSettings.controllerRotationInterval;
		}

		float transNum = 0f;
		for (int i = 0; i < driftSettings.controllerTransNumber; i++) {
			controllerTranslations [i] = transNum;
			transNum += driftSettings.controllerTransInterval;
		}

		float rRotNum = 0f;
		for (int i = 0; i < driftSettings.roomRotationNumber; i++) {
			roomRotations [i] = rRotNum;
			rRotNum += driftSettings.roomRotationInterval;
		}
			
	}

	// Update is called once per frame
	void Update () {
	}

	public void SetNewConditions() {
		
		if (balloonSpawn.subTrial == 1) {
			rnd = new System.Random();
		}

		if (controllerTranslate) {
			float upSign = rnd.Next(2) * 2 - 1;
			controllerUp = upSign * RandomControllerTranslation (rnd);
			while (controllerUp == prevControllerUp) {
				upSign = rnd.Next(2) * 2 - 1;
				controllerUp = upSign * RandomControllerTranslation (rnd);
			}
			prevControllerUp = controllerUp;

			float sideSign = rnd.Next (2) * 2 - 1;
			controllerSide = sideSign * RandomControllerTranslation (rnd);
			while (controllerSide == prevControllerSide) {
				sideSign = rnd.Next (2) * 2 - 1;
				controllerSide = sideSign * RandomControllerTranslation (rnd);
			}
			prevControllerSide = controllerSide;

			/*float forwardSign = rnd.Next (2) * 2 - 1;
			controllerForward = forwardSign * 0;
			while (controllerForward == prevControllerForward) {
				forwardSign = rnd.Next (2) * 2 - 1;
				controllerForward = forwardSign * RandomControllerTranslation (rnd);
			}
			prevControllerForward = controllerForward;*/
			// for now, set forward translation to 0 (do we want forward translation in this study?)
			controllerForward = 0;

			TranslateController (controllerUp, controllerSide, controllerForward);
		}

		if (controllerRotate) {
			
			float upRotSign = rnd.Next (2) * 2 - 1;
			controllerRotUp = upRotSign * RandomControllerRotation (rnd);
			while (controllerRotUp == prevControllerRotUp) {
				upRotSign = rnd.Next (2) * 2 - 1;
				controllerRotUp = upRotSign * RandomControllerRotation (rnd);
			}
			prevControllerRotUp = controllerRotUp;

			float sideRotSign = rnd.Next (2) * 2 - 1;
			controllerRotSide = sideRotSign * RandomControllerRotation (rnd);
			while (controllerRotSide == prevControllerRotSide) {
				sideRotSign = rnd.Next (2) * 2 - 1;
				controllerRotSide = sideRotSign * RandomControllerTranslation (rnd);
			}
			prevControllerRotSide = controllerRotSide;

			RotateController (controllerRotUp, controllerRotSide);
		}

		if (headsetRotate) {
			float rotSign = rnd.Next (2) * 2 - 1;
			headsetRot = rotSign * RandomRoomRotation (rnd);
			while (headsetRot == prevRoomRotation) {
				rotSign = rnd.Next (2) * 2 - 1;
				headsetRot = rotSign * RandomRoomRotation (rnd);
			}
			prevRoomRotation = headsetRot;

			RotateRoom (headsetRot);
		}
	}

	float RandomControllerTranslation(System.Random rnd) {
		int i = rnd.Next (driftSettings.controllerTransNumber);
		return controllerTranslations [i];
	}

	float RandomControllerRotation(System.Random rnd) {
		int i = rnd.Next (driftSettings.controllerRotationNumber);
		return controllerRotations [i];
	}

	float RandomRoomRotation(System.Random rnd) {
		int i = rnd.Next (driftSettings.roomRotationNumber);
		return roomRotations [i];
	}

	void RotateRoom(float degrees) {
		room.transform.rotation = initialRoomRotation;
		room.transform.position = initialRoomPosition;
		room.transform.RotateAround (cameraRig.transform.position, Vector3.down, degrees);
	}

	void RotateController(float upDegrees, float sideDegrees) {
		
		if (leftSword.activeInHierarchy) {
			leftSword.transform.localRotation = initialLeftSwordRotation;
			leftSword.transform.Rotate(new Vector3(upDegrees, 0, sideDegrees));
		} else if (rightSword.activeInHierarchy) {
			rightSword.transform.localRotation = initialRightSwordRotation;
			rightSword.transform.Rotate (new Vector3 (upDegrees, 0, sideDegrees));
		}
	}

	void TranslateController(float up, float side, float forward) {
		if (leftSword.activeInHierarchy) {
			float x = leftHand.transform.position.x;
			float y = leftHand.transform.position.y;
			float z = leftHand.transform.position.z;
			leftHand.transform.position = new Vector3(x + side, y + up, z + forward);
		} else if (rightHand.activeInHierarchy) {
			float x = rightHand.transform.position.x;
			float y = rightHand.transform.position.y;
			float z = rightHand.transform.position.z;
			rightSword.transform.position = new Vector3(x + side, y + up, z + forward);
		}
	}

	public List<string> GetDriftAmount() {
		string controllerUpS, controllerForwardS, controllerSideS, controllerRotUpS, controllerRotSideS, headsetRotS;
		if (controllerTranslate) {
			controllerUpS = controllerUp.ToString ();
			controllerForwardS = controllerForward.ToString ();
			controllerSideS = controllerSide.ToString ();
		} else {
			controllerUpS = "NONE";
			controllerForwardS = "NONE";
			controllerSideS = "NONE";
		}
		if (controllerRotate) {
			controllerRotUpS = controllerRotUp.ToString ();
			controllerRotSideS = controllerRotSide.ToString ();
		} else {
			controllerRotUpS = "NONE";
			controllerRotSideS = "NONE";
		}
		if (headsetRotate) {
			headsetRotS = headsetRot.ToString ();
		} else {
			headsetRotS = "NONE";
		}
		List<string> retList = new List<string> {
			controllerUpS,
			controllerForwardS,
			controllerSideS,
			controllerRotUpS,
			controllerRotSideS,
			headsetRotS
		};
		return retList;
	}
}
