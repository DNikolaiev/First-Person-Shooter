using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

	public float slowFactor;
	public float slowLength;
	public static TimeController instance=null;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);


	}
    // Update is called once per frame
    void Update() {
       
            Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	}

	public void SlowDownTIme(float factor, float length)
	{
		slowFactor = factor;
		slowLength = length;
		StartCoroutine ("Slower");
	}

	IEnumerator Slower()
	{
		Time.timeScale = slowFactor;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
		yield return new WaitForSeconds (slowLength);
		Time.fixedDeltaTime = 0.02f;

	}
}
