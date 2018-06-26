using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGun : Rifle {

    private void Start()
    {
        base.force = 125f;
        base.fireRate = 9;
        base.range = 30f;
        base.damage = 5;
        base.bullets = 10000;
        base.currentAmmo = 10000;
        base.magazineCapacity = 10000;
        base.maxAmmo = 10000;
        shotVolume = 15;
        nextTimeToFire = 0;
        fireSource = GetComponentInParent<AudioSource>();
        
    }
    public override void Fire()
    {
        if (Time.time >= nextTimeToFire)
        {
            base.Fire();
            nextTimeToFire = Time.time + 1f / fireRate;
        }
    }
}
