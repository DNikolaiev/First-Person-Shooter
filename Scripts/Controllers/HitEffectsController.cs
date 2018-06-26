using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HitEffectsController : MonoBehaviour {

    public GameObject woodImpactEffect;
    public GameObject dirtImpactEffect;
    public GameObject woodBulletHole;
    public GameObject dirtBulletHole;
    public GameObject metalImpactEffect;
    public GameObject metalBulletHole;
    public GameObject concreteImpactEffect;
    public GameObject concreteBulletHole;
    public GameObject sandImpactEffect;
    public GameObject sandBulletHole;
    public GameObject softImpactEffect;
    public GameObject softBulletHole;
    public GameObject fire;
    public GameObject fire2;
    public GameObject smoke;
    public GameObject[] bloodEffecs;
    public AudioClip[] woodImpactSounds;
    public AudioClip[] dirtImpactSounds;
    public AudioClip[] metalImpactSounds;
    public AudioClip[] concreteImpactSounds;
    public AudioClip[] softImpactSounds;
    public AudioClip[] glassImpactSounds;
    public AudioClip[] bloodImpactSounds;

    [SerializeField] private float radius;
    private GameObject bulletHole;

    void Start()
    {
        radius = 12f;
    }
    public void CheckOnSlice(RaycastHit hit)
    {
        if (hit.collider == null)
            return;

        if (hit.collider.tag == "Crate" || hit.collider.tag == "Wood")
        {
            InstantiateEffect(woodImpactEffect, null, hit);
            AudioController.instance.PlayRandomSound(woodImpactSounds, AudioController.instance.hitEffects, volume: 0.6f);

        }
        else if (hit.collider.tag == "Sand" || hit.collider.tag == "Rubber")
        {
            InstantiateEffect(softImpactEffect, null, hit);
            AudioController.instance.PlayRandomSound(softImpactSounds, AudioController.instance.hitEffects, volume: 0.6f);

        }
        else if (hit.collider.tag == "Metal" || hit.collider.tag == "Glass" || hit.collider.tag == "Turret")
        {
            InstantiateEffect(metalImpactEffect, null, hit);
            AudioController.instance.PlayRandomSound(metalImpactSounds, AudioController.instance.hitEffects, volume: 0.6f);

        }
        else if (hit.collider.tag == "Concrete")
        {
            InstantiateEffect(metalImpactEffect, null, hit);
            AudioController.instance.PlayRandomSound(concreteImpactSounds, AudioController.instance.hitEffects, volume: 0.6f);

        }

        else if (hit.collider.tag == "Enemy" || hit.collider.tag == "bodyPart" || hit.collider.tag=="Dead")
        {
            InstantiateEffect(bloodEffecs[Random.Range(0, bloodEffecs.Length)], null, hit);
            AudioController.instance.PlayRandomSound(bloodImpactSounds, AudioController.instance.hitEffects, volume: 0.6f);
        }
    }
	public void CheckOnHit(RaycastHit hit)
	{
		if (hit.collider == null)
			return;
		
		if (hit.collider.tag == "Crate") 
		{ 
			InstantiateEffect (woodImpactEffect, woodBulletHole, hit);
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
			AudioController.instance.PlayRandomSound (woodImpactSounds, AudioController.instance.hitEffects);
			else AudioController.instance.PlayRandomSound (woodImpactSounds, AudioController.instance.hitEffects, volume:0.2f);
		}
		if (hit.collider.tag == "Terrain")
		{
			InstantiateEffect (dirtImpactEffect, dirtBulletHole, hit);	
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
			AudioController.instance.PlayRandomSound (dirtImpactSounds, AudioController.instance.hitEffects);
			else AudioController.instance.PlayRandomSound (dirtImpactSounds, AudioController.instance.hitEffects, volume:0.3f);
		}
		if (hit.collider.tag == "Metal" || hit.collider.tag=="Turret" || hit.collider.tag=="Ladder") 
		{
			InstantiateEffect (metalImpactEffect, metalBulletHole, hit);
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
			AudioController.instance.PlayRandomSound (metalImpactSounds, AudioController.instance.hitEffects);
			else AudioController.instance.PlayRandomSound (metalImpactSounds, AudioController.instance.hitEffects, volume:0.3f);
		}
		if (hit.collider.tag == "Concrete") {
			InstantiateEffect (concreteImpactEffect, concreteBulletHole, hit);
			if (PlayerSettings.instance.IsPlayerAround (hit.collider.gameObject,radius))
			AudioController.instance.PlayRandomSound (concreteImpactSounds, AudioController.instance.hitEffects);
			else AudioController.instance.PlayRandomSound (concreteImpactSounds, AudioController.instance.hitEffects, volume:0.3f);
		}
		if (hit.collider.tag == "Sand") 
		{
			InstantiateEffect (sandImpactEffect, sandBulletHole, hit);
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
			AudioController.instance.PlayRandomSound (dirtImpactSounds, AudioController.instance.hitEffects);
			else AudioController.instance.PlayRandomSound (dirtImpactSounds, AudioController.instance.hitEffects, volume: 0.3f);
		}
		if (hit.collider.tag == "Rubber") 
		{
			InstantiateEffect (softImpactEffect, softBulletHole, hit);
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
			AudioController.instance.PlayRandomSound (softImpactSounds, AudioController.instance.hitEffects);
			else AudioController.instance.PlayRandomSound (softImpactSounds, AudioController.instance.hitEffects, volume:0.3f);
		}
		if (hit.collider.tag == "Glass") 
		{
			InstantiateEffect (metalImpactEffect, null, hit);
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
				AudioController.instance.PlayRandomSound (glassImpactSounds, AudioController.instance.hitEffects, volume:0.9f);
			else AudioController.instance.PlayRandomSound (glassImpactSounds, AudioController.instance.hitEffects, volume:0.3f);
		}

		if (hit.collider.tag == "Wood"|| hit.collider.tag=="Door") {
			InstantiateEffect (woodImpactEffect, null, hit);
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
			AudioController.instance.PlayRandomSound (woodImpactSounds, AudioController.instance.hitEffects);
			else AudioController.instance.PlayRandomSound (woodImpactSounds, AudioController.instance.hitEffects, volume:0.3f);
		} 

		if (hit.collider.tag == "Enemy" || hit.collider.tag=="bodyPart" || hit.collider.tag=="Dead") {
			 InstantiateEffect (bloodEffecs[Random.Range(0,bloodEffecs.Length)], null, hit);
			if(PlayerSettings.instance.IsPlayerAround(hit.collider.gameObject,radius))
				AudioController.instance.PlayRandomSound (bloodImpactSounds, AudioController.instance.hitEffects, volume:0.6f);
			else AudioController.instance.PlayRandomSound (bloodImpactSounds, AudioController.instance.hitEffects, volume:0.2f);
		}

	}



	public void SetOnFire(RaycastHit hit)
	{
		if (hit.collider == null)
			return;
        if (hit.collider.tag == "Crate" && !hit.collider.GetComponent<DestroyableObject>().wasIgnited
            || hit.collider.tag == "Enemy" && !hit.collider.GetComponent<Enemy>().wasIgnited)
        {

            PlayerSettings.instance.flamethrower.Ignite(hit.collider.gameObject);
            InstantiateEffect(fire, smoke, hit);

        }
       
		
	}



	private void InstantiateEffect(GameObject impactEffect, GameObject bulletHole, RaycastHit hit)
	{
		if (bulletHole != null) 
		{
			Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
			bulletHole = Instantiate (bulletHole, hit.point, Quaternion.LookRotation (hit.normal));
			bulletHole.transform.SetParent (hit.transform);

		}
        
        else
        {
           GameObject obj= Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(obj, 1.5f);
        }
	}


}
