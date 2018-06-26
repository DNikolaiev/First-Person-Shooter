using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	public int health;
	public int armor;
	public int maxHealth;
	public int maxArmor;
	protected float timeToHeal=0.2f;
	protected int healLeft=0;
	public bool isAlive=true;

	protected abstract void Die ();

	public void Heal(int healAmount)
	{
		if (maxHealth - health >= healAmount)
			StartCoroutine ("HealOverTime",  healAmount);
		else
			StartCoroutine("HealOverTime",   maxHealth - health);
	}

	IEnumerator HealOverTime(int healAmount)
	{
		healLeft += healAmount;
		while ( healLeft>0 && health < maxHealth) {
			health += 1;
			healLeft -= 1;
			yield return new WaitForSeconds (timeToHeal);
		}
		healLeft = 0;
	}

	public void Fortify(int armorAmount)
	{
		if (maxArmor - armor >= armorAmount)
			armor += armorAmount;
		else
			armor += maxArmor - armor;
	}

	public virtual void ApplyDamage(int damage)
	{
		int damageLeft = damage;
		bool absorbed = false;
		if (armor > 0 && armor - damage >= 0) {
			armor -= damage;
			absorbed = true;
		}
		else if(armor > 0 && armor-damage < 0) {
			damageLeft = damage - armor;
			armor = 0;
		}

		damage =  damageLeft;
		if(!absorbed)
			health -= damage;

		if (health <= 0) {
			health = 1;
			Die ();

		}
	}
		

}
