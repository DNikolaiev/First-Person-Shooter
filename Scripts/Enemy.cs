using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class Enemy : Character {
    public bool wasIgnited;
    public Transform[] waypoints;
    public GameObject player;
    public float sighRadius;
    public bool isChasing;
    public bool targetIsLocked;
    public string[] tagsToAttack;
    public GameObject radarMarker;
    protected Transform previousTarget;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float timeBetweenAttacks;
    protected int damage;
    protected float patroleSpeed;
    protected float speed;
    [SerializeField] protected bool patrolling;
    protected bool onWay;
    protected float chaseRadius;
    protected float stopDist;
    protected Enemy thisEnemy;
    protected int foVDifference;
    protected bool destroyAfterDeath;
    protected bool canMove;
    [SerializeField] protected Transform target;
    [SerializeField] protected float distance;
    [SerializeField] protected bool alarmed;
    [SerializeField] protected int fieldOfView;
    [SerializeField] protected int destPoint = 0;
    [SerializeField] protected Collider[] bodyParts;
    [SerializeField] protected AudioClip[] attackSounds;
    [SerializeField] protected AudioClip[] alarmSounds;
    [SerializeField] protected AudioClip[] idleSounds;
    [SerializeField] protected AudioClip[] chaseSounds;
    [SerializeField] protected AudioClip[] deathSounds;
    [SerializeField] protected AudioClip[] fallSounds;
    [SerializeField] protected AudioClip[] injuredSounds;
    [SerializeField] protected AudioSource source;
    [SerializeField] protected AudioSource attackSource;


    private float nextAttackTime = 0;
    protected bool CR_IsRunning = false;
    protected Character nearestCharacter = null;
    public abstract void FocusTarget();
    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stopDist = agent.stoppingDistance;
        thisEnemy = GetComponent<Enemy>();

    }
    protected void Start() {

        destroyAfterDeath = true;
        isChasing = false;
        targetIsLocked = false;
        isAlive = true;
        alarmed = false;
        wasIgnited = false;
        agent.autoBraking = false;
        patrolling = false;
        GotoNextPoint();
        AudioController.instance.PlayLoopSound(idleSounds[Random.Range(0, idleSounds.Length)], source, source.pitch, source.volume);
        onWay = false;
        previousTarget = target;
        AIController.instance.idleEnemies.Add(thisEnemy);  
    }

    protected void Patrole()
    {
        fieldOfView = foVDifference;
        AIController.instance.SetEnemyIdle(thisEnemy);
        animator.SetBool("Attack", false);
        if (waypoints.Length == 0) {
            agent.isStopped = true;
            agent.autoBraking = true;
            return;
        }
        else {
            agent.autoBraking = false;
            animator.SetBool("Walk", true);
            patrolling = true;
            agent.stoppingDistance = 0;
            agent.speed = patroleSpeed;
            if (Vector3.Distance(transform.position, waypoints[destPoint].position) <= agent.stoppingDistance + 0.5f) {
                if (!onWay)
                    GotoNextPoint();

            }
        }

    }

    protected void GotoNextPoint()
    {
        if (waypoints.Length == 0)
            return;
        onWay = true;
        destPoint++;
        if (destPoint == waypoints.Length)
            destPoint = 0;
        agent.SetDestination(waypoints[destPoint].position);


    }
    public void SearchForTarget()
    {
        if (CanSeeTarget() && distance <= sighRadius)
            FocusTarget();
    }
    
    protected virtual void ChaseTarget()
    {
        if (isAlive && targetIsLocked && target!=null)
        {
            AIController.instance.SetEnemyActive(thisEnemy); // add enemy to active enemies list

            if (!isChasing)
            {
                isChasing = true;
                AudioController.instance.PlayLoopSound(chaseSounds[Random.Range(0, chaseSounds.Length)], source, source.pitch, source.volume);
            }
            FaceTarget();
            alarmed = true;
            patrolling = false;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            fieldOfView = 360;
            if (distance <= stopDist + 1 && target!=null)
            {
                Attack();
            }
            else if (distance > stopDist + 1 || target==null)
            {
                animator.SetBool("Attack", false);
            }
        }
    }

    protected void FindClosestTarget()
    {
        if (!isAlive)
            return;

        float closestDistance = Mathf.Infinity;
        foreach (Character targetChar in AIController.instance.characters)
        {
            if (targetChar.name == thisEnemy.name && thisEnemy!=null)
                continue;
            float distance = Vector3.Distance(targetChar.transform.position, transform.position);
            
            if (distance < closestDistance && CheckTargetTag(targetChar.tag))
            {
                closestDistance = distance;
                nearestCharacter = targetChar;
            }
        }

        if (nearestCharacter != null && closestDistance <= sighRadius)
        {
            target = nearestCharacter.transform;
            previousTarget = target;
            targetIsLocked = true;
        }
    }
    protected virtual void TrackTarget()
    {
       
        if (!targetIsLocked || target==null)
            return;
        distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance >= chaseRadius && isChasing || !target.GetComponent<Character>().isAlive)
        {
            alarmed = false;
            isChasing = false;
            targetIsLocked = false;
            target = null;
            
            GotoNextPoint(); //<- disturbing
            
        }
    }
    private bool CheckTargetTag(string targetTag)
    {
        foreach (string tagStr in tagsToAttack)
        {
            if (tagStr == targetTag)
                return true;
        }
        return false;
    }

	
	protected bool CanSeeTarget()
	{
        if (target == null)
            return false;
		Vector3 rayDir = (target.position - transform.position).normalized;
		if (Vector3.Angle (rayDir, transform.forward) <= fieldOfView * 0.5f
			&& Vector3.Angle (rayDir, transform.forward) >= -fieldOfView * 0.5f)  {
			return true;
		} else
			return false;

	}
	public void SetTarget(Transform _target)
	{
		target = _target;
	}

	protected virtual void FaceTarget()
	{
        if (target == null )
            return;
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	void Attack()
	{
        if (target == null)
            return;
        animator.SetBool("Attack", true);
        animator.SetInteger("AttackNumber", Random.Range(0, 2));
        if (Time.time >= nextAttackTime)
        {
            AudioController.instance.PlayRandomSound(attackSounds, attackSource);
            AudioController.instance.PlayLoopSound(chaseSounds[Random.Range(0, chaseSounds.Length)], source, volume:source.volume);
            nextAttackTime = Time.time + timeBetweenAttacks;
            animator.SetBool("Attack", false);
        }

	}

    void DealDamage()
    {
        if(distance<=stopDist+1.5f && target.GetComponent<PlayerSettings>() != null)
            PlayerSettings.instance.ApplyDamage(damage);
        if (distance <= stopDist + 1.5f && target.GetComponent<Turret>() != null)
            target.GetComponent<Turret>().ApplyDamage(damage);
    }
    public void ApplyDamage(int damage, RaycastHit hit)
    {
        if(injuredSounds.Length!=0)
        AudioController.instance.PlayRandomSound(injuredSounds, attackSource, volume:source.volume);

        if (canMove)
        target = nearestCharacter.transform;

        targetIsLocked = true;
        if (animator != null)
        {
            animator.SetBool("Injured", true);
            StartCoroutine("SetIdle", "Injured");
        }
		if (hit.collider.name == bodyParts [0].name) {
			base.ApplyDamage (damage * 3);
			int rvalue = Random.Range (0, 3);
			if (!isAlive && rvalue==1 && gameObject!=null) 
				PlayerSettings.instance.CriticalKill ();
		}
		else if (hit.collider.name== bodyParts [1].name)
			base.ApplyDamage (damage);
		else
			base.ApplyDamage (damage / 2);
	}



	IEnumerator SetIdle(string Trigger)
	{
		yield return new WaitForFixedUpdate();
		animator.SetBool (Trigger, false);
	}

	void MarkAsDead(Transform obj)
	{
		radarMarker.SetActive (false);
		tag = "Dead";
		foreach (Transform child in obj) {
			child.tag = "Dead";
			if (obj.childCount == 0)
				return;
			else
			MarkAsDead (child);
		}
	}
	protected override void Die()
	{
        isAlive = false;
        target = null;
        agent.speed = 0;
        agent.isStopped = true;
        AIController.instance.idleEnemies.Remove(thisEnemy);
        AIController.instance.activeEnemies.Remove(thisEnemy);
        
        source.loop = false;
		AudioController.instance.PlayRandomSound (deathSounds, source, source.pitch,source.volume);

		MarkAsDead (transform);
		patrolling = false;
		alarmed = false;
        if (animator != null)
        {
            animator.SetInteger("DieNumber", Random.Range(0, 2));
            animator.SetBool("Die", true);
        }
		
        if(destroyAfterDeath)
		Destroy (gameObject, 5f);
	}

	void Fall()
	{
		AudioController.instance.PlayRandomSound (fallSounds, attackSource);
	}


}
