using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {

	// Use this for initialization
	void Start () {
        base.Start();
        chaseRadius = 30;
        damage = 10;
        timeBetweenAttacks = 1.5f;
        health = 120;
        armor = 0;
        maxHealth = 100;
        maxArmor = 0;

        speed = 1;
        sighRadius = 15;
        fieldOfView = 240;
        foVDifference = fieldOfView;
        patroleSpeed = speed;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        SearchForTarget();
        if (target!=null)
            TrackTarget();

        if (!alarmed && !isChasing)
            Patrole();


        if (alarmed || patrolling)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if (agent.remainingDistance >= 0.5)
            onWay = true;
        else
            onWay = false;

    }

    public override void FocusTarget()
    {
        ChaseTarget();
    }

    
}
