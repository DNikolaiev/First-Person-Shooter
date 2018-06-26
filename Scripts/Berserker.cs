using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker :Enemy {
	[SerializeField] Collider closestHealSpot;
	[SerializeField] bool healing=false;
	public ParticleSystem healEffect;
	void Awake()
	{
		base.Awake ();
	}
	// Use this for initialization
	void  Start () {

        base.Start();
        chaseRadius = 30;
		damage = 30;
		timeBetweenAttacks = 1.5f;
		armor = 150;
		maxHealth = 450;
		health = maxHealth;
		maxArmor = 0;

		speed = 3f;
		sighRadius = 10;
		fieldOfView = 200;
		foVDifference = fieldOfView;
		patroleSpeed = 2;
        canMove = true;
    }
	
	// Update is called once per frame
	void Update () {
       if(closestHealSpot==null)
        FindClosestTarget();
        SearchForTarget();
        TrackTarget();

        ControleHealth ();
		ControlePosition ();
        ControleAnimation();

        if (!alarmed&&!isChasing)
			Patrole ();
		if (agent.remainingDistance >= 0.5)
			onWay = true;
		else
			onWay = false;
		
	}
    public  override void FocusTarget()
    {
        ChaseTarget();
    }
    protected override void ChaseTarget()
    {
        if (!isAlive || target==null)
            return;
        if (!alarmed && targetIsLocked && CanSeeTarget())
        {
            StartCoroutine("Alarm");
            alarmed = true;
        }
        else
            animator.SetBool("Alarmed", false);

        if (!CR_IsRunning && target.tag!="Poison" && !healing && target!=null)
            base.ChaseTarget();
       
    }

    IEnumerator Alarm()
    {
        CR_IsRunning = true;
        animator.SetBool("Alarmed", true);
        AudioController.instance.PlayRandomSound(alarmSounds, source, volume:source.volume);
        source.loop = false;
        agent.isStopped = true;
        agent.speed = 0;
        yield return new WaitForSeconds(2.4f);

        agent.isStopped = false;

        agent.speed = speed;
        source.loop = true;
        CR_IsRunning = false;
        ChaseTarget();
    }
    private void ControleAnimation()
    {
        if (patrolling)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
        }
        else if (alarmed && !healing || isChasing && !healing)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);
        }
        else if (healing)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Healing", true);
            FaceTarget();
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Healing", false);
        }

    }
    void ControlePosition()
	{
        if (target == null)
            return;
		
		if (Vector3.Distance (target.position, transform.position) <= stopDist +1.5f && targetIsLocked) {
			agent.speed = 1;
			agent.acceleration = 120;
			agent.angularSpeed = 1000;
            
		} else {
			agent.angularSpeed = 120;
			agent.acceleration = 200;
			agent.speed = speed;

		}

	}
	void ControleHealth()
	{
		if (health <= maxHealth * 0.1)
			agent.speed = 1;
		else
			agent.speed = speed;
		if (health <= maxHealth * 0.4) {
			FindHealSpot ();
		} 
	}

	void FindHealSpot()
	{
		closestHealSpot = null;
		alarmed = true;

		Collider[] colliders = (Physics.OverlapSphere (transform.position, sighRadius));
		foreach (Collider healSpot in colliders) {
			if (healSpot.tag == "Poison") {
				closestHealSpot = healSpot;
			}
			if (colliders.Length == 0)
				closestHealSpot = null;
		}

		if (closestHealSpot != null) {
			target = closestHealSpot.transform;
			agent.SetDestination (target.transform.position);
		} else {
            target = previousTarget;
			return;
		}
	}
	protected override void Die()
	{
        agent.isStopped = true;
		base.Die ();
	}

	void OnTriggerEnter(Collider trigger)
	{
		if (trigger.tag == "Poison") {
			Heal (maxHealth);
           // animator.SetBool("Healing", true);
			healEffect.gameObject.SetActive (true);
			healEffect.Play ();
			healEffect.Emit (10);
			healing = true;
            if (!CR_IsRunning)
                StartCoroutine("HealOff");
            else CR_IsRunning = true;
            closestHealSpot = null;
		}
	}

    private void OnTriggerStay(Collider other)
    {
        closestHealSpot = null;
        agent.SetDestination(target.position);
    }

    IEnumerator HealOff()
	{
        CR_IsRunning = true;
		agent.speed = 0;
		agent.isStopped = true;
		yield return new WaitForSeconds (0.1f);
		healing = false;
		animator.SetBool ("Healing", false);
		agent.speed = speed;
		agent.isStopped = false;
        CR_IsRunning = false;
      
	}
}
