using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {

	public int health=10;
	public GameObject[] itemArray;
	public GameObject crackedVersion;
    
	public bool wasHit=false;
	public bool wasIgnited=false;
	public bool alive=true;
    
    void Start()
	{
		health = 60;
	}
	public void ApplyDamage(int damage)
	{
		health -= damage;
		if (health <= 0) {
			if(this.gameObject.tag=="Crate")
			SpawnItem ();
			Die ();
		}
			
	}

	public void Die()
	{
		alive = false;
        if (crackedVersion != null)
        {
            Instantiate(crackedVersion, transform.position, transform.rotation);
           
        }
		Destroy (this.gameObject);
	}

	public void SpawnItem()
	{
		int randValue = Random.Range (0, itemArray.Length);
		GameObject obj = Instantiate (itemArray [randValue], transform.position, Quaternion.identity);
		if (obj.tag == "Fuel")
			obj.transform.Rotate (new Vector3 (-90, 0, 0));
		else if (obj.tag == "HealthItem")
			obj.transform.Rotate (new Vector3 (90, 0, 0));
	}
}
