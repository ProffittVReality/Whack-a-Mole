using UnityEngine;
using System.Collections;

public class BalloonPop : MonoBehaviour {

	public SphereCollider balloonCol;
	public GameObject balloon;
	public AudioClip popSound;
	private AudioSource source;

	public TrialManagerScript trialManager;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Sword" && !balloon.GetComponent<BalloonScript>().popAttempted) {
			balloonCol.enabled = false;
			if (!balloon.GetComponent<BalloonScript>().hasPopped) {
				trialManager.popCount += 1; // inform the trial manager of balloon pop
				balloon.GetComponent<BalloonScript>().hasPopped = true; // NOTE: must reset this to false when respawning balloon

				source.PlayOneShot (popSound, 1F);
			}
			StartCoroutine (ExecuteAfterTime (0.1f));
		}
	}

	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		balloon.SetActive (false);
	}
}
