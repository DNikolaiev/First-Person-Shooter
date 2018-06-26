using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {
	// Use this for initialization
	[SerializeField] private float timeBeforeDestroy=12f;
	void Start () {
		Destroy (this.gameObject, timeBeforeDestroy);

	}
	



	}
