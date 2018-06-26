using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Flamethrower : Weapons {

	public ParticleSystem muzzleFire;
	public AudioClip inFire;
	public AudioClip outFire;
	public int timeToBurn;

	[SerializeField] private HitEffectsController hitEffect;
	private float  nextTimeToFire=0f;
	private bool fireSound=false;
	private bool looping = true;

	private int timerBurn;
	private float sourceTime;

	[SerializeField] private int burnDamage;

	public override void Fire()
	{
		if (bullets <= 0 && currentAmmo > 0 && !isReloading) {
			StartCoroutine ("Reload");
		} else if (currentAmmo == 0 && bullets == 0) {
			fire = false;
			AudioController.instance.StopLoopSound (AudioController.instance.weapon);
			StartCoroutine("SetIdle","Fire");
		}

		if (fire)
		{
			//play animations
			animator.SetBool ("Fire", true);
			muzzleFire.gameObject.SetActive (true);
			if (!fireSound) {
				AudioController.instance.PlayLoopSound (shots [Random.Range (0, shots.Length)], AudioController.instance.weapon);
				fireSound = true;
				looping = true;
			}
			
			bullets--;
			hitEffect.SetOnFire (base.Shoot ());
		}
	}
		

	private IEnumerator Reload()
	{
		int whatLeft = 0;
		AudioController.instance.StopLoopSound (AudioController.instance.weapon);
		controller.GetComponent<WeaponController> ().enabled = false;
		AudioController.instance.PlayRandomSound (reloads, AudioController.instance.reload,0.7f);
		muzzleFire.gameObject.SetActive (false);
		animator.SetBool ("Fire", false);
		isReloading = true;
		animator.SetBool ("Reload", true);
		fire = false;

		yield return new WaitForSeconds (4f);
		if (currentAmmo >= magazineCapacity)
			whatLeft = magazineCapacity - bullets;
		else
			whatLeft = currentAmmo;
		bullets += whatLeft;
		currentAmmo -= whatLeft;
		animator.SetBool ("Reload", false);
		fire = true;
		isReloading = false;
		controller.GetComponent<WeaponController> ().enabled = true;
	}

	public void Ignite (GameObject obj)
	{
		
		StartCoroutine ("Burn", obj);
	}
	IEnumerator Burn(GameObject obj)
	{
		timerBurn = timeToBurn;
		if (obj.GetComponent<Enemy> ()) {
			
			Enemy enemyToBurn = obj.GetComponent<Enemy> ();

			if (enemyToBurn.wasIgnited == false) {
				while (timerBurn > 0) {
					
					enemyToBurn.wasIgnited = true;
					if (enemyToBurn.isAlive)
						enemyToBurn.ApplyDamage (burnDamage);
					timerBurn--;
					yield return new WaitForSeconds (1);
				}

				enemyToBurn.wasIgnited = false;

			}
		} else if (obj.GetComponent<DestroyableObject> ()) {
			DestroyableObject objToBurn = obj.GetComponent<DestroyableObject> ();

			if (objToBurn.wasIgnited == false) {
				while (timerBurn > 0) {

					objToBurn.wasIgnited = true;
					if (objToBurn.alive)
						objToBurn.ApplyDamage (burnDamage);
					timerBurn--;
					yield return new WaitForSeconds (1);
				}

				objToBurn.wasIgnited = false;
			}
		}
	}



	// Use this for initialization
	void Start () {
		base.force = 5f;
		base.fireRate = 15;
		base.range = 11f;
		base.damage = 10;
		base.bullets = 60;
		base.currentAmmo = 240;
		base.magazineCapacity = 100;
		base.maxAmmo = 300;
		base.aimingFOV = 45;
		timeToBurn = 4;
		burnDamage = 20;
		shotVolume = 6;
	}

	// Update is called once per frame
	void Update () {
		bulletsText.text = bullets + "/" + currentAmmo;
		if (bullets < 0)
			muzzleFire.gameObject.SetActive (false);
		if (fireSound) {
			sourceTime = AudioController.instance.weapon.time;
			if (!AudioController.instance.weapon.isPlaying && sourceTime == 0) 
				fireSound = false;
			if(!looping)
			AudioController.instance.StopLoopSound (AudioController.instance.weapon);
		}
		InputSystem ();
	}

	IEnumerator SetIdle(string Trigger)
	{
		yield return new WaitForFixedUpdate();
		animator.SetBool (Trigger, false);
	}

	protected override void InputSystem ()
	{
		if (Input.GetButton ("Fire1") && Time.time >= nextTimeToFire) {
				nextTimeToFire = Time.time + 1f / fireRate;
				Fire ();
			}
		if (Input.GetButtonUp ("Fire1")&&!isReloading&&bullets>0) {
			animator.SetBool ("Fire", false);
			looping = false;
			AudioController.instance.PlaySound (outFire, AudioController.instance.weapon,volume:0.5f);
		}
		
		if (Input.GetButtonDown ("Fire1")&&!isReloading && bullets>0) {
			AudioController.instance.PlayRandomSound (shots, AudioController.instance.weapon);

		}
		//reload
		if (Input.GetKeyDown (KeyCode.R)&& bullets < magazineCapacity && !isReloading && currentAmmo>0) 
			StartCoroutine ("Reload");
		
		//aim
		if (Input.GetButtonDown ("Fire2")) {

			base.StartAiming ();
		}
		if (Input.GetButtonUp ("Fire2")) {
			base.StopAiming ();
		}

	}

	protected override void OnEnable()
	{
		muzzleFire.gameObject.SetActive (true);
		base.OnEnable ();
	}


}
