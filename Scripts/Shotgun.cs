using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shotgun : Weapons {

	public ParticleSystem muzzleFire;

	[SerializeField] private HitEffectsController hitEffect;
	[SerializeField] private float xSpread;
	[SerializeField] private float ySpread;
	private float  nextTimeToFire=0f;
	[SerializeField] private int _numberOfShots;
	RaycastHit[] hits;

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
			AudioController.instance.PlayRandomSound (shots, AudioController.instance.weapon);	
			bullets--;
			hits = Shoot (_numberOfShots);
			foreach (RaycastHit elem in hits) {
				hitEffect.CheckOnHit (elem);
			}

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
		AudioController.instance.PlayRandomSound (reloads, AudioController.instance.reload,0.85f);
		isReloading = true;
		animator.SetBool ("Reload", true);
		fire = false;

		yield return new WaitForSeconds (1.5f);
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
	// Use this for initialization
	void Start () {
		
		base.force = 300f;
		base.fireRate = 1;
		base.range = 6f;
		base.damage = 20;
		base.bullets = 2;
		base.currentAmmo = 40;
		base.magazineCapacity = 2;
		base.maxAmmo = 40;
		base.aimingFOV = 45;
		xSpread = 0.15f;
		ySpread = 0.15f;
		_numberOfShots = 15;
		shotVolume = 25;
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
		if (Input.GetKeyDown (KeyCode.R)&& bullets < magazineCapacity && !isReloading&& currentAmmo>0)
			StartCoroutine ("Reload");
		//aim
		if (Input.GetButtonDown ("Fire2")) {

			base.StartAiming ();
		}
		if (Input.GetButtonUp ("Fire2")) {
			base.StopAiming ();
		}

	}

	protected virtual RaycastHit[] Shoot(int numberOfShots)
	{
		trail.Play ();
		trail.Emit (10);
		RaycastHit hit;
		DestroyableObject obj = null;
		RaycastHit[] hitArray=new RaycastHit[numberOfShots];
		AudioController.instance.PlayRandomSound (shots, AudioController.instance.weapon);
		for (int i = 0; i < numberOfShots; i++) {
			Vector3 shotPos = new Vector3 (Random.Range (-xSpread, xSpread), Random.Range (-ySpread, ySpread), 0f) + cam.transform.forward;
			if (Physics.Raycast (cam.transform.position, shotPos, out hit, range)) {
				Debug.Log (hit.collider.name);
				hitArray [i] = hit;

				 obj = hit.transform.GetComponent<DestroyableObject> ();
				Explosion _exlposion = hit.transform.GetComponent<Explosion> ();
				Enemy _enemy = hit.transform.GetComponent<Enemy> ();
                PlayerSettings player = hit.transform.GetComponentInChildren<PlayerSettings>();
                Turret turret = hit.transform.GetComponentInParent<Turret>();

                if (turret != null)
                { turret.ApplyDamage(damage, hit); }
                if (player != null && player.isAlive)
                {
                    player.ApplyDamage(damage);
                }

                if (_exlposion != null && !_exlposion.exploaded) 
				{
					_exlposion.Explode ();
					_exlposion.exploaded = true;
				}
				if (obj != null&& !obj.wasHit && _exlposion==null)
				{
					obj.ApplyDamage (obj.health);
					obj.wasHit = true;
				}
					
				if (_enemy != null && _enemy.isAlive) {
					enemyHitted = true;
					_enemy.ApplyDamage (damage);
				}
				if (hit.rigidbody != null) {
					hit.rigidbody.AddForce (-hit.normal * force);

				}

				NoiseController.instance.SpreadNoise (shotVolume/2, hit.transform.position);

			}
		}
		NoiseController.instance.SpreadNoise (shotVolume, transform.position);

		return hitArray;
	}

	void OnEnable()
	{
		muzzleFire.gameObject.SetActive (false);
		bulletsImage.enabled = true;
		crosshair.enabled = true;
	}


}
