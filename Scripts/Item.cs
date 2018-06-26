using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	[SerializeField] private int healAmount=20;
	[SerializeField] private int amorAmount=20;
	[SerializeField] private int bulletsRifle=30;
	[SerializeField] private int grenades=1;
	[SerializeField] private int shellsShotgun = 2;
	[SerializeField] private int fuel=50;
	[SerializeField] private float degreesPerSecond = 17.0f;





	private Vector3 position;
	private bool isFull=false;


	void OnTriggerEnter(Collider other)
	{
		if (other.tag=="Player") {
			if (this.gameObject.tag == "HealthItem") {
				if (PlayerSettings.instance.health < PlayerSettings.instance.maxHealth) {
					PlayerSettings.instance.Heal (healAmount);
					AudioController.instance.PlayRandomSound (AudioController.instance.healthSounds, AudioController.instance.other);
				}
				else
					isFull = true;
			}
			if (this.gameObject.tag == "ArmorItem") {
				if (PlayerSettings.instance.armor < PlayerSettings.instance.maxArmor){
					PlayerSettings.instance.Fortify (amorAmount);
					AudioController.instance.PlayRandomSound (AudioController.instance.armorSounds, AudioController.instance.other);
				}
				else
					isFull = true;
			}

			if (this.gameObject.tag == "BulletsRifle") {
				
				if (PlayerSettings.instance.rifle.GetCurrentAmmo () < PlayerSettings.instance.rifle.GetMaxAmmo ()) {
					PlayerSettings.instance.RefillAmmo (PlayerSettings.instance.rifle, bulletsRifle);
					AudioController.instance.PlayRandomSound (AudioController.instance.equipmentSounds, AudioController.instance.other);
				}
					else
						isFull = true;
			}
			if (this.gameObject.tag == "ShellsShotgun") {

				if (PlayerSettings.instance.shotgun.GetCurrentAmmo () < PlayerSettings.instance.shotgun.GetMaxAmmo ()) {
					PlayerSettings.instance.RefillAmmo (PlayerSettings.instance.shotgun, shellsShotgun);
					AudioController.instance.PlayRandomSound (AudioController.instance.equipmentSounds, AudioController.instance.other);
				}
				else
					isFull = true;
			}
			if (this.gameObject.tag == "Fuel") {

				if (PlayerSettings.instance.flamethrower.GetCurrentAmmo () < PlayerSettings.instance.flamethrower.GetMaxAmmo ()) {
					PlayerSettings.instance.RefillAmmo (PlayerSettings.instance.flamethrower, fuel);
					AudioController.instance.PlayRandomSound (AudioController.instance.equipmentSounds, AudioController.instance.other);
				}
				else
					isFull = true;
			}

			if (this.gameObject.tag == "Explosives") {
				if (PlayerSettings.instance.grenades < PlayerSettings.instance.maxGrenades) {
					PlayerSettings.instance.RefillGrenades (grenades);
					AudioController.instance.PlayRandomSound(AudioController.instance.equipmentSounds,AudioController.instance.other);
				}
				else
					isFull = true;
			}
			if (!isFull)
				Destroy (this.gameObject);
			else
				isFull = false;
		}
		
	}

	void Start()
	{
		
		position = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
		transform.position = position;
	}


	void Update()
	{
		//rotate an item
		transform.Rotate (new Vector3 (0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

	}
	

}
