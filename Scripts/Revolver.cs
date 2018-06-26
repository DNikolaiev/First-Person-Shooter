using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Revolver : Weapons {


	public ParticleSystem muzzleFire;
	[SerializeField] private HitEffectsController hitEffect;
	private float  nextTimeToFire=0f;

	public override void Fire()
	{
		if (bullets <= 0 && currentAmmo > 0 && !isReloading) {
			StartCoroutine ("Reload");
		}
		else if (currentAmmo == 0&&bullets==0)
			fire = false;

		if (fire)
		{
			//play animations
			animator.SetBool ("Fire", true);
			StartCoroutine ("SetIdle","Fire");
			muzzleFire.gameObject.SetActive (true);
			muzzleFire.Play ();
			AudioController.instance.PlayRandomSound (shots, AudioController.instance.weapon,volume:0.6f);	
			bullets--;
			hitEffect.CheckOnHit(base.Shoot());
		}
	}

	IEnumerator SetIdle(string Trigger)
	{
		yield return new WaitForFixedUpdate();
		animator.SetBool (Trigger, false);
	}

	 IEnumerator Reload()
	{
		int whatLeft = 0;
		controller.GetComponent<WeaponController> ().enabled = false;
		AudioController.instance.PlayRandomSound (reloads, AudioController.instance.reload,0.7f);
		isReloading = true;
		animator.SetBool ("Reload", true);
		fire = false;

		yield return new WaitForSeconds (2f);
		if (currentAmmo >= magazineCapacity)
			whatLeft = magazineCapacity - bullets;
		else
			whatLeft = currentAmmo;
		bullets += whatLeft;
		currentAmmo -= whatLeft;
		animator.SetBool ("Reload", false);
		fire = true;
		AddAmmo (GetMaxAmmo() - GetCurrentAmmo());
		isReloading = false;
		controller.GetComponent<WeaponController> ().enabled = true;
	}
	// Use this for initialization
	void Start () {
		base.force = 200f;
		base.fireRate = 2;
		base.range = 25f;
		base.damage = 40;
		base.bullets = 6;
		base.currentAmmo = 60;
		base.magazineCapacity = 6;
		base.maxAmmo = 60;
		base.aimingFOV = 45;
		base.shotVolume = 20;
	}

	// Update is called once per frame
	void Update () {
		bulletsText.text = bullets + "/" + currentAmmo;
		InputSystem ();
	}

	protected override void InputSystem ()
	{ 
		
			if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
				nextTimeToFire = Time.time + 1f / fireRate;
				Fire ();
			}
		 // reload
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


}
