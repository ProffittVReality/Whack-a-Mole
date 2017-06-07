using UnityEngine;
using System.Collections;

public class BalloonPop : MonoBehaviour {

	public SphereCollider balloonCol;
	public GameObject balloon;
	public AudioClip popSound;
	private AudioSource source;

	public bool hasPopped = false;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {
		balloonCol.enabled = false;
		if (!hasPopped) {
			source.PlayOneShot (popSound, 1F);
			hasPopped = true; // NOTE: must reset this to false when respawning balloon
		}
		StartCoroutine(ExecuteAfterTime(0.1f));
	}

	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		balloon.SetActive (false);
	}
}
