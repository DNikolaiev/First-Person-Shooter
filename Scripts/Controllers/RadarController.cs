using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour {

	public float radarDistance;
	public List <Enemy> enemyList;
	public Transform player;
	void Update()
	{
		Collider[] colliders = Physics.OverlapSphere (player.position, radarDistance);
		foreach (Collider col in colliders) {
			Enemy foe = col.gameObject.GetComponent<Enemy> ();
            if (foe != null && !enemyList.Contains(foe))
            {
                enemyList.Add(foe);
            }
            else if(foe==null)
                foe = col.gameObject.GetComponentInChildren<Enemy>();

            if (col.tag == "Turret" && !enemyList.Contains(foe) && foe!=null)
            {
                enemyList.Add(foe);

            }
		}
		foreach (Enemy enemy in enemyList)
		{
            if (!enemy.isAlive)
                enemyList.Remove(enemy);
			if (enemy!=null && Vector3.Distance (player.position, enemy.transform.position) > radarDistance) {
				enemy.radarMarker.SetActive (false);

				enemyList.Remove (enemy);
                
			} else
				enemy.radarMarker.SetActive (true);
		}
	}

	void Start()
	{
		radarDistance = 20f;
	}
}
