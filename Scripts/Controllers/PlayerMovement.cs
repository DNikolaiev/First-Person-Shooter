using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MoveController))]
public class PlayerMovement : MonoBehaviour {
	[SerializeField] private float speed = 5f;
	[SerializeField] private float mouseSensetivity=3f;
	private MoveController movController;

	// Use this for initialization
	void Start () {
		movController = GetComponent<MoveController> ();
	}
	
	// Update is called once per frame
	void Update () {
		float xMov = Input.GetAxisRaw ("Horizontal");
		float zMov = Input.GetAxisRaw ("Vertical");

		Vector3 movHorizontal = transform.right * xMov;
		Vector3 movVertical = transform.forward * zMov;
		//final movement vector
		Vector3 _velocity = (movVertical + movHorizontal).normalized * speed/20;
		movController.Move (_velocity);

		float yRot = Input.GetAxisRaw ("Mouse X");
		float xRot = Input.GetAxisRaw ("Mouse Y");
		// Calculate rotation as 3D vector
		Vector3 _rotation = new Vector3 (0f, yRot, 0) * mouseSensetivity;
		Vector3 _cameraRotation = new Vector3 (xRot, 0f, 0) * mouseSensetivity;

		//Apply player rotation
		movController.Rotate(_rotation);

		//Apply camera rotation
		movController.RotateCamera (_cameraRotation);

	}
}
