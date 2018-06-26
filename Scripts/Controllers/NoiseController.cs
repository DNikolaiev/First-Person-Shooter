using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour {
	public static NoiseController instance=null;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}
    public void SpreadNoise(float radius, Vector3 fromWhere)
    {
        Collider[] nearbyTransformObjects = Physics.OverlapSphere(fromWhere, radius);
        foreach (Collider nearbyObject in nearbyTransformObjects) {
            if (nearbyObject.GetComponent<Enemy>() && nearbyObject.tag == "Enemy")
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();
                enemy.FocusTarget();
            }
          
		}
	}
}
