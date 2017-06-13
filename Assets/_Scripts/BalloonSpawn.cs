﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Balloons {
	public GameObject balloon0;
	public GameObject balloon1;
	public GameObject balloon2;
	public GameObject balloon3;
	public GameObject balloon4;
	public GameObject balloon5;
	public GameObject balloon6;
	public GameObject balloon7;
	public GameObject balloon8;
	public GameObject balloon9;
	public GameObject balloon10;
	public GameObject balloon11;
}

public class BalloonSpawn : MonoBehaviour {

	public float lowIntervalLimit;
	public float highIntervalLimit;

	public Balloons balloonSet;

	[HideInInspector]
	public GameObject activeBalloon;

	[HideInInspector]
	public GameObject[] balloons;

	private int i = 0;
	private int[] indices;

	//[HideInInspector]
	public bool balloonActive = false; // true if any balloon is active
	public bool balloonSleeping = true; // true if active balloon is sleeping

	[HideInInspector]
	public int subTrial;

	bool trialEnd = false;

	// Use this for initialization
	void Start () {
		// create array of balloons
		balloons = new GameObject[12] {balloonSet.balloon0, balloonSet.balloon1, balloonSet.balloon2, balloonSet.balloon3, balloonSet.balloon4, balloonSet.balloon5, balloonSet.balloon6, balloonSet.balloon7, balloonSet.balloon8, balloonSet.balloon9, balloonSet.balloon10, balloonSet.balloon11};
		// make sure all balloons begin inactive
		foreach (GameObject balloon in balloons) {
			balloon.SetActive (false);
		}

		// generate first order for spawning
		// TODO: move this to Update (solution: put in StartTrial)
		GenerateOrder ();
	}

	// Update is called once per frame
	void Update () {
		Debug.Log (activeBalloon);
	}

	public bool RunTrial(float activeTime) {
		// returns true when all 12 balloons have appeared (round is over)
		if (i < 12) {
			trialEnd = false;
		}
		if ((i < 12) && !balloonActive) {
			// generate random time between balloons
			float intervalTime = Random.Range(lowIntervalLimit, highIntervalLimit);

			StartCoroutine (HoldForBalloon (intervalTime));

			balloons [indices[i]].GetComponent<BalloonScript>().hasPopped = false;
			balloons [indices[i]].GetComponent<BalloonScript>().popAttempted = false;
			balloons [indices [i]].GetComponent<BalloonScript> ().popTime = Time.time + intervalTime + activeTime;
			//activeBalloon = balloons [indices [i]];

			StartCoroutine (StayWoke (intervalTime, activeTime));

			// set conditions for next balloon to spawn
			balloonActive = true;
		}


		if (i == 12) {
			// delay true return to allow late hits on last balloon
			StartCoroutine (DelayTrue (2f));
		}
		return trialEnd;
	}

	IEnumerator DelayTrue(float time) {
		// change trialEnd to true after time seconds
		yield return new WaitForSeconds (time);
		trialEnd = true;
	}
		
	public void StartTrial() {
		i = 0;
		GenerateOrder ();
	}

	// helper function for RunTrial
	IEnumerator StayWoke(float intervaltime, float activeTime) {
		// balloon becomes inactive after time seconds
		int index = indices [i];
		yield return new WaitForSeconds(intervaltime + activeTime);
		balloons[index].SetActive (false);
		balloonActive = false;
		i += 1; // put in this function to delay index increase until balloon disappears
	}

	// helper function for RunTrial
	IEnumerator HoldForBalloon(float time) {
		// holds for time then generates a balloon
		int index = indices [i];
		balloonSleeping = true;
		yield return new WaitForSeconds (time);
		balloons [index].SetActive (true);

		balloonSleeping = false;
		activeBalloon = balloons [indices [i]];
		subTrial = i+1; // because i is the index and indices go from 0 not 1
	}

	// helper function for NewTrial
	void GenerateOrder() {
		// Generates a new random order for balloons to spawn
		indices = new int[12] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
		System.Random rnd = new System.Random ();

		for (int i = 0; i < 12; i++) {
			int randomVal = rnd.Next (12);
			while (System.Array.IndexOf (indices, randomVal) > -1) {
				randomVal = rnd.Next (12);
			}
			indices [i] = randomVal;
		}
	}
	

}
