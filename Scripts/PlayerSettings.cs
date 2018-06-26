using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSettings : Character {

    #region publicVar
    public int grenades;
	public int maxGrenades;
	public Grenade grenadePrefab;
	public Rifle rifle;
	public Revolver revolver;
	public Shotgun shotgun;
	public Flamethrower flamethrower;
    public int hackRange;
    public int energy;
    public int energyPerHackConsumed;
    public int maxEnergy;
    public WeaponController wc;
	public RawImage redScreen;
	public RawImage redHit;
    public RawImage adrenalinScreen;
	public AudioClip criticalHitSound;
	public AudioClip heartbeatFast;
	public AudioClip deathSound;
	public AudioClip[] playerInjured;
	public Text healthText;
    public Image healthBar;
    public Image energyBar;
	public Text armorText;
    public Image armorBar;
    public Image adrenalineBar;
    public Image hackingBar;
    public Text hackingText;
	public Text grenadeText;
    public bool isInBattle;

	private bool isAdrenalined;
    public bool IsAdrenalined { get { return isAdrenalined; } }
    #endregion
    #region privateVar
    private bool isHealed=false;
    private bool recharging;
    private float timeToHack;
    private float timeButtonPressed;
    [SerializeField] private float adrenaline;
    private int damageMultiplier;
    private float nextTime=0;
	private Camera cam;
	private Grenade temporaryGrenade;

	private Weapons[] weapons = new Weapons[4];
	private int[] damageDef;
	private int[] fireRateeDef;
    private bool damageIncreased;
    #endregion
    public static PlayerSettings instance=null;
	void Awake()
	{
		weapons [0] = revolver;
		weapons [1] = rifle;
		weapons [2] = shotgun;
		weapons [3] = flamethrower;
		damageDef = new int[weapons.Length];
		fireRateeDef = new int[weapons.Length];

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

	}
	// Use this for initialization
	void Start () {
		StartCoroutine ("ScrollAllWeapon");
		Invoke ("GetDefaultValues", 1f);
		damageIncreased = false;

        hackRange = 25;
        timeToHack = 2f;
		damageMultiplier = 2;
		adrenaline = 0;
		cam = GetComponent<Camera> ();
		maxArmor = 100;
		maxHealth = 100;
        maxEnergy = 100;
		maxGrenades = 3;
		health = 100;
		armor = 0;
		grenades = 3;
        energy = 100;
        energyPerHackConsumed = 50;
		nextTime = 0f;
		isAlive = true;
		isAdrenalined = false;
        recharging = false;
	}

#region start
	private void GetDefaultValues()
	{
		for (int i = 0; i < weapons.Length; i++) {
			damageDef [i] = weapons [i].damage;
			fireRateeDef [i] = weapons [i].fireRate;
		}
	}
	IEnumerator ScrollAllWeapon()
	{
		foreach (Weapons weapon in weapons) {
			weapon.gameObject.SetActive (true);
			yield return null;
			weapon.gameObject.SetActive (false);
			revolver.gameObject.SetActive (true);
		}
	}
    #endregion
    #region Energy
    private void ControleEnergyLevel()
    {
        energyBar.fillAmount = energy * 0.01f;
        if (energyBar.rectTransform.parent.gameObject.activeInHierarchy )
        {
            UIController.instance.Fade(energyBar.rectTransform.parent.GetComponent<Image>(), 1f, 0);
            UIController.instance.Fade(energyBar, 1f, 0);
          
        }
        if (energy < maxEnergy && !recharging)
        {
            StartCoroutine("RestoreEnergy");
        }
    }
    IEnumerator RestoreEnergy()
    {
        recharging = true;
        while (energy < maxEnergy)
        {
            energy += 1;
            yield return new WaitForSeconds(2f);
        }
        recharging = false;
        yield return null;
    }

    private void DisplayEnergyBar()
    {
        energyBar.rectTransform.parent.gameObject.SetActive(true);
        while (energyBar.color.a < 0.95f)
        {
            UIController.instance.Fade(energyBar.rectTransform.parent.GetComponent<Image>(), 2f, 1);
            UIController.instance.Fade(energyBar, 2f, 1);
        }
    }

    private void HackTurret()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hackRange))
        {
            if (hit.transform.tag == "Turret" && !hit.transform.GetComponentInParent<Turret>().isHacked)
            {
                hit.transform.GetComponentInParent<Turret>().AllyWithPlayer();
                energy -= energyPerHackConsumed;
                hackingText.gameObject.SetActive(false);
                DisplayEnergyBar();
                hackingBar.rectTransform.parent.gameObject.SetActive(false);
            }
        }
    }
    #endregion
    #region Health
    public override void ApplyDamage(int damage)
	{
		if (Time.time > nextTime) {
			AudioController.instance.PlayRandomSound (playerInjured, AudioController.instance.player);
			nextTime = Time.time + 1f;
		}
		UIController.instance.StartCoroutine ("HitScreen", redHit);
		adrenaline += 5;
		base.ApplyDamage (damage);
	}



	void ControleHealthLevel()
	{
        armorBar.fillAmount = armor * 0.01f;
        healthBar.fillAmount = health * 0.01f;
		healthText.text = health.ToString();
		armorText.text = armor.ToString();

		if (isAdrenalined && health <= 10) {
				health = 10;
		}
		if (health <= maxHealth * 0.3f && armor==0 ) {
			if (isHealed) {
				AudioController.instance.PlayLoopSound (heartbeatFast, AudioController.instance.heartBeat, volume: 1.5f);
				isHealed = false;
			}
			UIController.instance.Fade (redScreen, 1, 1);
		} else {
			AudioController.instance.StopLoopSound (AudioController.instance.player);
			isHealed = true;
			UIController.instance.Fade (redScreen, 1, 0);

		}
	}
#endregion

#region Adrenaline
	void ControleAdrenalineLevel()
	{
        if(isAdrenalined)
            UIController.instance.Fade(adrenalinScreen, 1, 1);
        else UIController.instance.Fade(adrenalinScreen, 1, 0);

        adrenalineBar.fillAmount = adrenaline * 0.01f;
		adrenaline = Mathf.Clamp (adrenaline, 0, 100);

		if (isAdrenalined && !damageIncreased) {
			damageIncreased = true;
			for (int i = 0; i < weapons.Length; i++) {
				weapons [i].damage = damageDef [i] * damageMultiplier;
				weapons [i].fireRate = fireRateeDef [i] * damageMultiplier;
			}
		} else if (!isAdrenalined && damageIncreased) {
			damageIncreased = false;
			for (int i = 0; i < weapons.Length; i++) {
				weapons [i].damage = damageDef [i];
				weapons [i].fireRate = fireRateeDef [i];
			}
		}
		
			if (adrenaline >= 99) {
				StartCoroutine ("AdrenalineRush");
				isAdrenalined = true;
			}
            else if (adrenaline < 10 && isAdrenalined) {
				isAdrenalined = false;
			}
		
	}

	IEnumerator AdrenalineRush()
	{
		while (adrenaline > 0) {

			adrenaline -= 10;
			yield return new WaitForSeconds (1);
		}
	}


	public void CriticalKill()
	{
		adrenaline += 20;
		AudioController.instance.PlaySound (criticalHitSound, AudioController.instance.enemy, 0.75f);
		TimeController.instance.SlowDownTIme(0.1f,4f);

	}
		
	#endregion

	public bool IsPlayerAround (GameObject obj, float radius)
	{
		if (Vector3.Distance(transform.position,obj.transform.position) <= radius)
			return true;
		else
			return false;
	}

	void Update()
	{
        if (!isAlive)
        {
            Time.timeScale = 0;
            return;
        }
        RaycastHit hit;
        ControleEnergyLevel();
		ControleHealthLevel ();
		ControleAdrenalineLevel ();
		
		grenadeText.text = grenades.ToString();
		//throw a grenade
		Vector3 spawnPoint = cam.transform.position + cam.transform.forward - cam.transform.up/2 - cam.transform.right / 2;
		
		if (Input.GetKeyDown (KeyCode.G) && grenades>0) {
			grenades--;
			temporaryGrenade =Instantiate (grenadePrefab, spawnPoint,cam.transform.rotation);
			temporaryGrenade.transform.parent = this.gameObject.transform;

		}

		if (Input.GetKeyUp (KeyCode.G))
			try{
			temporaryGrenade.GetComponent<Rigidbody>().isKinematic=false;
			temporaryGrenade.transform.parent=null;
			temporaryGrenade.ThrowGrenade ();
		}
		catch{}
        if (Input.GetKeyDown(KeyCode.Q))
        {
            timeButtonPressed = Time.time;
            hackingBar.fillAmount = 0;
            DisplayEnergyBar();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            timeButtonPressed = 0;
            hackingBar.rectTransform.parent.gameObject.SetActive(false);
            hackingText.gameObject.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Q) && energy >= energyPerHackConsumed)
        {
            DisplayEnergyBar();
            if (timeButtonPressed == 0)
                return;
            
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hackRange)) {
                if (hit.collider.tag == "Turret" && !hit.collider.GetComponentInParent<Turret>().isHacked)
                {

                    hackingBar.rectTransform.parent.gameObject.SetActive(true);
                    hackingText.gameObject.SetActive(true);
                    hackingBar.fillAmount += timeToHack / (timeToHack * 1000) + Time.deltaTime/2;
                    if (Time.time >= timeButtonPressed + timeToHack && Time.time < timeButtonPressed + timeToHack * 1.5f)
                        HackTurret();
                }
                else
                {
                    hackingBar.rectTransform.parent.gameObject.SetActive(false);
                    hackingText.gameObject.SetActive(false);
                    timeButtonPressed = 0;
                    hackingBar.fillAmount = 0;
                }
            }
            else {
                hackingBar.rectTransform.parent.gameObject.SetActive(false);
                hackingText.gameObject.SetActive(false);
                timeButtonPressed = 0;
                hackingBar.fillAmount = 0;
            }
        }
       
        
    }

    
#region Ammo
	public void RefillAmmo(Weapons weapon, int ammo)
	{
		
		if (weapon.GetMaxAmmo() - weapon.GetCurrentAmmo() >= ammo)
			weapon.AddAmmo (ammo);
			else
			weapon.AddAmmo (weapon.GetMaxAmmo() - weapon.GetCurrentAmmo()); 
			
	}
	public void RefillGrenades(int _grenades)
	{
		if (maxGrenades - grenades >= _grenades)
			grenades += _grenades;
		else
			grenades += maxGrenades - grenades;
	}
		
#endregion
	protected override void Die()
	{
		Debug.Log ("YOU DIED");
		AudioController.instance.StopLoopSound (AudioController.instance.player);
		AudioController.instance.player.clip = null;
		AudioController.instance.PlaySound(deathSound, AudioController.instance.player);
		health = 0;
		isAlive = false;
        ButtonController.instance.canv.enabled = true;
        
	}


}

