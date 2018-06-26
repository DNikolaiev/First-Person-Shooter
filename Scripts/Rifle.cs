using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Rifle : Weapons {

	public ParticleSystem muzzleFire;
	[SerializeField] private bool automatic;
	[SerializeField] private HitEffectsController hitEffect;
    [SerializeField] protected AudioSource fireSource;
	protected float  nextTimeToFire=0f;

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
			AudioController.instance.PlayRandomSound (shots, fireSource);	
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

		yield return new WaitForSeconds (2.3f);
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
		base.force = 125f;
		base.fireRate = 9;
		base.range = 30f;
		base.damage = 20;
		base.bullets = 30;
		base.currentAmmo = 120;
		base.magazineCapacity = 30;
		base.maxAmmo = 120;
		base.aimingFOV = 30;
		automatic = true;
		shotVolume = 15;
        fireSource = AudioController.instance.weapon;
	}
	
	// Update is called once per frame
	void Update () {
		bulletsText.text = bullets + "/" + currentAmmo;
		InputSystem ();
			
	}
			
	protected override void InputSystem ()
	{ // change fire mode
		if (Input.GetKeyDown (KeyCode.V))
			automatic = !automatic;
		if (PlayerSettings.instance.IsAdrenalined)
			automatic = true;
		
		if (automatic) {
			if (Input.GetButton ("Fire1") && Time.time >= nextTimeToFire) {
				damage = 20;
				nextTimeToFire = Time.time + 1f / fireRate;
				Fire ();
			}
		} else if (!automatic) {
			
			if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
				damage = 30;
				nextTimeToFire = Time.time + 1f / fireRate;
				Fire ();

			}
		} // reload
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
