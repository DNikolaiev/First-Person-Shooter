using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlashLightController : BaseController {

	public Light light;
	public AudioClip flashlightSound;
	public Text percentage;
	public RawImage flashlight;
	[SerializeField]
	private int lightCapacity;
	private bool isActiveFlashlight = false;
	[SerializeField]
	private int timeToLight=30;

	void Awake()
	{
		light = GetComponent<Light> ();
	}
	public void Start()
	{
		
		lightCapacity = timeToLight;
		SetActiveFlashlight (false);
	}

	public void Update()
	{
		if (lightCapacity == timeToLight) {
			UIController.instance.Fade (percentage, 1f, 0);
		} else
			UIController.instance.Fade (percentage, 1, 1);
		percentage.text = lightCapacity*100/timeToLight + "%";
		if (Input.GetKeyDown (KeyCode.T)) {
			isActiveFlashlight = !isActiveFlashlight;
			if (isActiveFlashlight) {
				TurnOn ();
			} else
				TurnOff ();
		}
		CheckFlashLightBattery ();
		//Add flashlight's working time, batteries?
	}
	void CheckFlashLightBattery ()
	{
		if (lightCapacity == 0)
			TurnOff ();
	}
	private void SetActiveFlashlight(bool state)
	{
		flashlight.enabled = state;
		light.enabled = state;
		AudioController.instance.PlaySound (flashlightSound, AudioController.instance.player);
	}

	public override void TurnOn ()
	{
		if (Enabled)
			return;
		else {
			base.TurnOn ();
			SetActiveFlashlight (true);
			StopAllCoroutines ();
			StartCoroutine (ConsumeBattery ());
		}
	}

	public override void TurnOff ()
	{
		if (!Enabled)
			return;
		else {
			base.TurnOff ();
			SetActiveFlashlight (false);
			StopAllCoroutines ();
			StartCoroutine (RechargeBattery ());
		}
	}

	IEnumerator ConsumeBattery()
	{
		while (lightCapacity > 0) {
			lightCapacity--;
			yield return new WaitForSeconds (1);
		}
	}

	IEnumerator RechargeBattery()
	{
		while (lightCapacity < timeToLight) {
			lightCapacity++;
			yield return new WaitForSeconds (1);
		}
	}
}
