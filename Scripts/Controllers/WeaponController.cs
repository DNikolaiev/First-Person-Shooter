using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
	private int selectedWeapon;
	private Weapons activeWeapon;
	// Use this for initialization
	void Start () {
		selectedWeapon = 0;
		SelectWeapon ();
	}
	public Weapons TrackActiveWeapon()
	{
		if (selectedWeapon == 0)
			activeWeapon = PlayerSettings.instance.revolver;
		if (selectedWeapon == 1)
			activeWeapon = PlayerSettings.instance.rifle;
		if (selectedWeapon == 2)
			activeWeapon = PlayerSettings.instance.shotgun;
		if (selectedWeapon == 3)
			activeWeapon = PlayerSettings.instance.flamethrower;
		return activeWeapon;

	}
	// Update is called once per frame
	void Update () {
		int previousSelectedWeapon = selectedWeapon;
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
			if (selectedWeapon >= transform.childCount - 1)
				selectedWeapon = 0;
			else
				selectedWeapon++;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
			if (selectedWeapon <= 0)
				selectedWeapon = transform.childCount - 1;
			else
				selectedWeapon--;
		}

		if (Input.GetKeyDown (KeyCode.Alpha1))
			selectedWeapon = 0;
		if (Input.GetKeyDown (KeyCode.Alpha2)&& transform.childCount>=2)
			selectedWeapon = 1;
		if (Input.GetKeyDown (KeyCode.Alpha3)&&transform.childCount>=3)
			selectedWeapon = 2;
		if (Input.GetKeyDown (KeyCode.Alpha4)&&transform.childCount>=4)
			selectedWeapon = 3;

		if (previousSelectedWeapon != selectedWeapon)
			SelectWeapon ();
	}

	private void SelectWeapon()
	{
		int i = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedWeapon) 
				weapon.gameObject.SetActive (true);
			else
				weapon.gameObject.SetActive (false);
			i++;
		}
	}
}
