using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Turret : Enemy {
    Quaternion originRot;
    public ParticleSystem deathEffect;
    public ParticleSystem hackEffect;
    public AudioClip[] hackSounds;
    public TurretGun gun;
    public Camera cam;
    public bool isHacked;
    
    private void Awake()
    {
        base.Awake();
        gun = transform.parent.GetComponentInChildren<TurretGun>();
    }
    // Use this for initialization
    void Start()
    {
        base.Start();
        chaseRadius = 20;
        damage = 10;
        timeBetweenAttacks = 1f;
        health = 500;
        armor = 500;
        maxHealth = 500;
        maxArmor = 500;
        originRot = transform.rotation;
        speed = 0;
        sighRadius = 15;
        fieldOfView = 240;
        foVDifference = fieldOfView;
        patroleSpeed = speed;
        destroyAfterDeath = false;
        tagsToAttack[0] = "Enemy";
        tagsToAttack[1] = "MainCamera";
        canMove = false;
        isHacked = false;
        gun.fireRate = 3;
    }
  
	// Update is called once per frame
	void Update () {
        FindClosestTarget();
        SearchForTarget();
        TrackTarget();
	}

    protected override void TrackTarget()
    {
        if (CanSeeTarget() && target.GetComponent<Character>().isAlive)
        {
            isChasing = true;
            alarmed = true;
        }
        else
        {
            isChasing = false;
            alarmed = false;
            ReturnToOrigin();//return origin position
        }
        base.TrackTarget();
    }
    private void ReturnToOrigin()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, originRot, Time.deltaTime * 3f);
        
    }
    public override void FocusTarget()
    {
        if (target.GetComponent<Character>().isAlive || target.GetComponentInChildren<Character>().isAlive)
        {
            FaceTarget();
            gun.Fire();
           
        }
        else return;
    }
    protected override void Die()
    {
        deathEffect.Play();
        Invoke("DeathEffectOff", 1f);
        base.Die();
    }
    private void DeathEffectOff()
    {
        deathEffect.gameObject.SetActive(false);
    }
    public void AllyWithPlayer()
    {
        if (isHacked)
            return;
        Debug.Log("Enter");
        int pos = Array.IndexOf(tagsToAttack, "MainCamera");
        tagsToAttack[pos] = "Crate";
        target = null;
        isHacked = true;
        radarMarker.GetComponent<MeshRenderer>().material.color = Color.green;
        radarMarker.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);
        hackEffect.Play();
        AudioController.instance.PlayRandomSound(hackSounds, attackSource, volume: attackSource.volume);

    }


}
