using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour {

	public Transform cam;


	[SerializeField] private float yRecoilRate=0.1f;
	private Vector3 recoilVector;

	public void ApplyRecoil(float weaponForce)
	{
		recoilVector = new Vector3 (0, -Random.Range (-yRecoilRate, yRecoilRate), 0f);
		cam.transform.Rotate (recoilVector);
		StartCoroutine ("ReturnPosition");

	}

	IEnumerator ReturnPosition()
	{
		yield return new WaitForSeconds (0.1f);
		cam.transform.Rotate (-recoilVector);
	}
		
		
}
