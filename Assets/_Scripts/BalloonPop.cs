using UnityEngine;
using System.Collections;

public class BalloonPop : MonoBehaviour {

	public SphereCollider balloonCol;
	public GameObject balloon;
	public GameObject popParticle;
	public AudioClip popSound;
	private AudioSource source;

	public TrialManagerScript trialManager;

	public GameObject swordTipL;
	public GameObject swordTipR;

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

			if (swordTipL.activeInHierarchy) {
				balloon.GetComponent<BalloonScript>().entryPosition = swordTipL.transform.position;
			}
			else if (swordTipR.activeInHierarchy) {
				balloon.GetComponent<BalloonScript>().entryPosition = swordTipR.transform.position;
			}

			if (!balloon.GetComponent<BalloonScript>().hasPopped) {
				trialManager.popCount += 1; // inform the trial manager of balloon pop
				balloon.GetComponent<BalloonScript>().hasPopped = true;

				source.PlayOneShot (popSound, 1F);
			}
			StartCoroutine (AnimateAfterTime (0.02f));
			StartCoroutine (PopAfterTime (0.1f));
		}
	}

	IEnumerator PopAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		balloon.SetActive (false);
	}

	IEnumerator AnimateAfterTime(float time) {
		yield return new WaitForSeconds(time);
		popParticle.GetComponent<ParticleSystem> ().Stop ();
		popParticle.GetComponent<ParticleSystem> ().Play ();
	}
}
