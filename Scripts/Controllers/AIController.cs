using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIController : MonoBehaviour {
	public List<Enemy> activeEnemies = new List<Enemy> ();
	public List<Enemy> idleEnemies=new List<Enemy>();
    public List<Character> characters = new List<Character>();
    public PlayerSettings player;
	public static AIController instance = null;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
        


	}
    private void Start()
    {
       
    }
    public void SetEnemyActive(Enemy thisEnemy)
    {
        idleEnemies.Remove(thisEnemy);

        if (!activeEnemies.Contains(thisEnemy))
           activeEnemies.Add(thisEnemy);
    }

    public void SetEnemyIdle(Enemy thisEnemy)
    {
        activeEnemies.Remove(thisEnemy);
        if (!idleEnemies.Contains(thisEnemy))
            idleEnemies.Add(thisEnemy);
    }

    private void TrackCharacters()
    {
        characters = activeEnemies.ConvertAll(x => (Character)x);
        characters.AddRange(idleEnemies.ConvertAll(x => (Character)x));
        characters.Add(player as Character);
    }
    private void Update()
    {
        TrackCharacters();
        PlayerSettings.instance.isInBattle = activeEnemies.Count == 0 ? false : true;
    }

}
