using UnityEngine;
using System.Collections;

public class BalloonSpawn : MonoBehaviour {

	[HideInInspector]
	public float intervalTime;
	public float startIntervalTime;

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

	[HideInInspector]
	public GameObject[] balloons;

	private int i = 0;
	private int[] indices;

	// Use this for initialization
	void Start () {
		balloons = new GameObject[10] {balloon0, balloon1, balloon2, balloon3, balloon4, balloon5, balloon6, balloon7, balloon8, balloon9};

		// set initial interval time
		intervalTime = startIntervalTime;

		// generate first order for spawning
		GenerateOrder ();

		StartTrial ();
	}

	void StartTrial() {
		// starts a new spawning sequence
		i = 0;
		// repeats function "Spawn" every intervalTime seconds
		InvokeRepeating ("Spawn", 0f, intervalTime);
	}

	void Spawn() {
		// activates the balloon at index i in balloons
		if (i < 10) {
			int index = indices [i];
			balloons [index].SetActive (true);
			i += 1;
		}
	}

	void GenerateOrder() {
		// Generates a new random order for balloons to spawn
		indices = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
		System.Random rnd = new System.Random ();

		for (int i = 0; i < 10; i++) {
			int randomVal = rnd.Next (10);
			while (System.Array.IndexOf (indices, randomVal) > -1) {
				randomVal = rnd.Next (10);
			}
			indices [i] = randomVal;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
