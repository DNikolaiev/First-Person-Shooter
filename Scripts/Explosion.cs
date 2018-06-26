using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Explosion : MonoBehaviour {
	[SerializeField] protected GameObject[] explosionEffects;
	[SerializeField] protected float radius;
	[SerializeField] protected float explosionForce;
	[SerializeField] protected int explosionDamage;
	[SerializeField] protected float explosionVolume;
	public AudioSource source;
	public AudioClip[] explosionSounds;
	public AudioClip[] farExplosionSounds;

	public bool exploaded=false;
	protected void Start()
	{
		explosionVolume = 60f;
		radius = 5f;
		explosionDamage = 150;
	}
	public void Explode()
	{

        Destroy(gameObject);
		if (PlayerSettings.instance.IsPlayerAround (this.gameObject, radius + 10)) {
			AudioController.instance.PlayRandomSound (explosionSounds, AudioController.instance.explosion);
		}
		else
			AudioController.instance.PlayRandomSound (farExplosionSounds, AudioController.instance.explosion);
		
		int randomNumber = Random.Range (0, explosionEffects.Length);
		GameObject explosionChoosen = explosionEffects [randomNumber];
		Instantiate (explosionChoosen, transform.position, transform.rotation);

	// Find objects to destroy, destroy them and apply force to shattered pieced
		 Collider[] collidersToDestroy = Physics.OverlapSphere (transform.position, radius);

		if (collidersToDestroy.Length>2)
			TimeController.instance.SlowDownTIme (0.35f, 2f);
		
		foreach(Collider nearbyObject in collidersToDestroy){
			
			DestroyableObject dest= nearbyObject.GetComponent<DestroyableObject> ();
			if (dest != null)
				dest.ApplyDamage (explosionDamage);
			
			if ((nearbyObject.tag == "Enemy" || nearbyObject.tag=="Turret"))
				CalculateAreaDamage (nearbyObject.gameObject, radius,0.1f);
			
			if (nearbyObject.GetComponent<Explosion> () && nearbyObject.name!=name)
				nearbyObject.GetComponent<Explosion> ().Invoke ("Explode", 0.15f);
		}
			

		Collider[] collidersToMove = Physics.OverlapSphere (transform.position, radius);

		foreach (Collider nearbyObject in collidersToMove) {
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody> ();
			 if (rb!=null)
				rb.AddExplosionForce (explosionForce, transform.position, radius);
		}

		if (PlayerSettings.instance.IsPlayerAround (transform.gameObject, radius)) {
			CalculateAreaDamage (PlayerSettings.instance.gameObject, radius,20);
			CameraShaker.instance.ShakeCamera (0.6f,1.5f,1f);
		}

		NoiseController.instance.SpreadNoise (explosionVolume, transform.position);
		Destroy (this.gameObject);
	}

	private void CalculateAreaDamage(GameObject target, float radius, float damageReduction)
	{
        Character character;
		float dif = Vector3.Distance (transform.position, target.transform.position);
		 character = target.GetComponent<Character> ();
        if (target.tag == "Turret")
        {
            Character turret = target.GetComponentInParent<Character>();
            if (turret != null)
            {
                turret.ApplyDamage(explosionDamage * 2);
            }
        }
        
        //Check if target in explosion radius 
        if (dif <= radius && target.tag != "Turret")
        {
            if(explosionDamage - (int)((dif) * damageReduction) > 0)
            character.ApplyDamage(explosionDamage - (int)((dif) * damageReduction));
           
        }
	}
		
}
