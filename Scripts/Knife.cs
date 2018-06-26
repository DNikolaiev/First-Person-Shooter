using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapons {
   public HitEffectsController hitFX;
    private float nextTimeToFire=0f;

	// Use this for initialization
	void Start () {
        base.force = 50f;
        base.fireRate = 1;
        base.range = 2f;
        base.damage = 15;
        base.bullets = 0;
        base.currentAmmo = 0 ;
        base.magazineCapacity = 0;
        base.maxAmmo = 0;
        base.aimingFOV = 60;
        base.shotVolume = 2;
        
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update () {
     
        InputSystem();
	}
    public override void Fire()
    {
        animator.Play("Knife_hit", 0);
        Invoke("SetIdle", 0.01f);
        hitFX.CheckOnSlice(Shoot());
    }
    void SetIdle()
    {
        animator.SetBool("Hit",false);
    }
    protected override RaycastHit Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, ~(1 << LayerMask.NameToLayer("GunLayer"))))
        {

            DestroyableObject obj = hit.transform.GetComponent<DestroyableObject>();
            Enemy _enemy = hit.transform.GetComponent<Enemy>();
            PlayerSettings player = hit.transform.GetComponentInChildren<PlayerSettings>();
            Turret turret = hit.transform.GetComponentInParent<Turret>();

            if (turret != null )
            { turret.ApplyDamage(damage/2, hit); }

            if (obj != null)
            {
                obj.ApplyDamage(damage*3);
            }
            if (player != null && player.isAlive && !transform.GetComponentInParent<PlayerSettings>())
            {
                player.ApplyDamage(damage * 3);
            }

            if (_enemy != null && _enemy.isAlive)
            {
                if (_enemy.isChasing)
                    _enemy.ApplyDamage(damage, hit);
                else _enemy.ApplyDamage(damage * 20, hit);
            }
           
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);

            }
        }
        else
        {

        }
        NoiseController.instance.SpreadNoise(shotVolume, transform.position);
        return hit;
    }
    protected override void InputSystem()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }
    }
    private void FixedUpdate()
    {
        
    }
}
