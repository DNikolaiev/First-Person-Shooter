using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapons : MonoBehaviour {
	[SerializeField] protected float force;
	public int fireRate;
	[SerializeField] protected float range;
	public int damage;
	[SerializeField] protected int bullets;
	[SerializeField] protected int currentAmmo;
	[SerializeField] protected int magazineCapacity;
	[SerializeField] protected int maxAmmo;
	[SerializeField] protected float aimingFOV;
	[SerializeField] protected int shotVolume;
	[SerializeField] protected Camera cam;
	[SerializeField] protected WeaponController controller;

	[SerializeField] protected AudioClip[] shots;
	[SerializeField] protected AudioClip[] reloads;
	[SerializeField] protected ParticleSystem trail;
	public bool isReloading=false;
	public Text bulletsText;
	public RawImage bulletsImage;
	public Camera gunCam;
	public RawImage crosshair;
	public RawImage hitCrosshair;
    public Text helpText;
	public Animator animator;

	protected bool fire=true;
	protected bool isAiming = false;
	protected bool enemyHitted=false;
	public abstract void Fire ();
	protected abstract void InputSystem ();


	// Perform a Raycast Shot
	protected virtual RaycastHit Shoot()
	{
		RaycastHit hit; 
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, range, ~(1 << LayerMask.NameToLayer ("GunLayer")))) {
			
			DestroyableObject obj = hit.transform.GetComponent<DestroyableObject> ();
			Explosion _exlposion = hit.transform.GetComponent<Explosion> ();
			Enemy _enemy = hit.transform.GetComponent<Enemy> ();
            PlayerSettings player = hit.transform.GetComponentInChildren<PlayerSettings>();
            Turret turret = hit.transform.GetComponentInParent<Turret>();
           
            if ( turret != null)
            { turret.ApplyDamage(damage, hit); }

            if (obj != null) {
				obj.ApplyDamage (damage);
			}
            if (player != null && player.isAlive && !transform.parent.GetComponentInParent<PlayerSettings>())
            {
                player.ApplyDamage(damage/2);
            }

            if (_enemy != null && _enemy.isAlive) {
				enemyHitted = true;

				_enemy.ApplyDamage (damage, hit);
			} else
				enemyHitted = false;
         

            if (_exlposion != null) {
				_exlposion.Explode ();
			}
			if (hit.rigidbody != null) {
				hit.rigidbody.AddForce (-hit.normal * force);

			}
			NoiseController.instance.SpreadNoise (shotVolume/2, hit.transform.position);
		} 
		else 
		{
			
		}
		NoiseController.instance.SpreadNoise (shotVolume, transform.position);
		return hit;
	}
		

	public int GetCurrentAmmo()
	{
		return currentAmmo;
	}
	public void AddAmmo(int value)
	{
		currentAmmo += value;
	}
	public int GetMaxAmmo()
	{
		return maxAmmo;
	}

	protected virtual void OnEnable()
	{
        if (crosshair != null)
        {
            bulletsText.text = bullets + "/" + currentAmmo;
            bulletsImage.enabled = true;
            crosshair.enabled = true;
        }


	}
	protected void OnDisable()
	{
        if (crosshair != null)
        {
            bulletsImage.enabled = false;
            crosshair.enabled = false;
        }

	}
	void Start()
	{
        if (bulletsImage!=null)
		    bulletsImage.enabled = false;
	}

	void Awake()
	{
        if (bulletsImage != null)
            bulletsImage.enabled = false;
	}
	void FixedUpdate()
	{
        if (crosshair == null || tag=="Knife")
            return;
		RaycastHit hit; 
		if (enemyHitted) {
			StartCoroutine ("SetTriggerFalse");
			UIController.instance.Fade (hitCrosshair, 8, 1);

		} else
			UIController.instance.Fade (hitCrosshair, 4, 0);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (hit.collider.tag == "Enemy" || hit.collider.tag == "bodyPart" || hit.collider.tag == "Turret")
            {
                crosshair.color = Color.red;
                if (hit.collider.tag == "Turret" && PlayerSettings.instance.energy >= PlayerSettings.instance.energyPerHackConsumed && !hit.collider.GetComponentInParent<Turret>().isHacked && helpText!=null)
                {
                    helpText.text = "Q";
                    helpText.color = Color.cyan;
                    helpText.gameObject.SetActive(true);
                }
            }
            else if (range >= Vector3.Distance(hit.transform.position, transform.position))
            {
                crosshair.color = Color.white;
                helpText.gameObject.SetActive(false);
            }
            else
            {
                crosshair.color = Color.white;
                helpText.gameObject.SetActive(false);
            }
        }
        else
        {
            crosshair.color = Color.white;
            helpText.gameObject.SetActive(false);
        }
	}

	IEnumerator SetTriggerFalse()
	{
		yield return new WaitForSeconds (0.4f);
		enemyHitted = false;
	}
	protected void StartAiming()
	{
		isAiming = true;
		animator.SetBool ("Aim", isAiming);
		if (isAiming) {
			cam.fieldOfView = aimingFOV;
			gunCam.fieldOfView = aimingFOV;
		} else if (isAiming && bullets <= 0)
		StartCoroutine ("Reload");	else {
			cam.fieldOfView = 60;
			gunCam.fieldOfView = 60;
		}

		crosshair.enabled = false;
		trail.gameObject.SetActive (false);
	}
	protected void StopAiming()
	{
		cam.fieldOfView = 60;
		gunCam.fieldOfView = 60;
		isAiming = false;
		crosshair.enabled = true;
		animator.SetBool ("Aim", isAiming);
		trail.gameObject.SetActive (true);
	}
}
