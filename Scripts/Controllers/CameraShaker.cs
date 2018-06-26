using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

	 float shakeTime=2f;
	 float shakeAmount=2f;
	 float shakeSpeed=2f;

	public static CameraShaker instance=null;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	public void ShakeCamera(float _shakeTime, float _shakeAmount, float _shakeSpeed)
	{
		shakeTime = _shakeTime;
		shakeAmount = _shakeAmount;
		shakeSpeed = _shakeSpeed;
		StartCoroutine ("Shake");
	}

	 IEnumerator Shake()
	{
		Vector3 originPosition = transform.localPosition;
		float elapsedTime = 0f;
		while (elapsedTime < shakeTime) {
			Vector3 randomPoint = originPosition + Random.insideUnitSphere * shakeAmount;
			transform.localPosition = Vector3.Lerp (transform.localPosition, randomPoint, shakeTime * Time.deltaTime);
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		transform.localPosition = originPosition;

	}


}
