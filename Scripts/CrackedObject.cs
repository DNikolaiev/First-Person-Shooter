using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedObject : MonoBehaviour {
    public AudioSource source;
    public AudioClip[] cracksounds;
    // Use this for initialization
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    void Start () {
        AudioController.instance.PlayRandomSound(cracksounds, source);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
