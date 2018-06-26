using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {
	
	public int damage;
	private float nextTime;

	void Start()
	{
		damage = 1;
		nextTime = 0;
	}

	void Update()
	{
		if (PlayerSettings.instance.IsPlayerAround (this.gameObject, 2f)  && Time.time > nextTime) {
			nextTime = Time.time+1;
			PlayerSettings.instance.ApplyDamage (damage);

		}
	}
}
