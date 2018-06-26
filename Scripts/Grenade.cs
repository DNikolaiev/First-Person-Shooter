using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosion {
	[SerializeField] private int delayTime = 4;
	[SerializeField] private int throwForce=5;
	private Camera cam;
	private bool hasExploded;
	private int countdown;

	public AudioClip throwSound;
	public AudioClip spawnSound;
	public AudioClip[] collisionSounds;
	public AudioClip[] metalCollisionSounds;

	// Use this for initialization
	void Start () {
		base.Start ();
		cam = GetComponentInParent<Camera> ();
		countdown = delayTime;
		AudioController.instance.PlaySound (spawnSound, source);
		StartCoroutine ("StartCountdown");
	}
	
	IEnumerator StartCountdown()
	{
		while (countdown >= 0) {
			countdown--;
			yield return new WaitForSeconds (1f);
		}
		Explode ();
	}

	public void ThrowGrenade()
	{
		AudioController.instance.PlaySound (throwSound, source);
		Rigidbody rb = this.gameObject.GetComponent<Rigidbody> ();
		rb.AddForce (cam.transform.forward*throwForce, ForceMode.VelocityChange);
		StartCoroutine ("PerformRotation");

	}

	IEnumerator PerformRotation()
	{
		float angle = 0.1f;
		while (countdown >= 0) {
			transform.RotateAround (new Vector3(-Time.fixedDeltaTime, 0, 0), angle);
			yield return new WaitForFixedUpdate ();

		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (PlayerSettings.instance.IsPlayerAround (this.gameObject, radius)) {
			if (other.gameObject.tag == "Metal")
				AudioController.instance.PlayRandomSound (metalCollisionSounds, source);
			else
				AudioController.instance.PlayRandomSound (collisionSounds, source);
		} 
		else {
			if (other.gameObject.tag == "Metal")
				AudioController.instance.PlayRandomSound (metalCollisionSounds, source,volume:0.15f);
			else
				AudioController.instance.PlayRandomSound (collisionSounds, source, volume:0.15f);
		}
	}
}
