using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MoveController : MonoBehaviour {
	private Vector3 velocity=Vector3.zero;
	private Vector3 rotation=Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;
	private Rigidbody rb;
	[SerializeField] private Camera cam;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}
	public void RotateCamera(Vector3 _cameraRotation)
	{
		cameraRotation = _cameraRotation;
	}
	public void Move(Vector3 _velocity)
	{
		velocity=_velocity;
	}

	void FixedUpdate()
	{
		PerformMovement ();
		PerformRotation ();

	}

	void PerformRotation()
	{
		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation));
		if (cam != null) {
		
			cam.transform.Rotate (-cameraRotation);
		}
	}


	void PerformMovement()
	{
		if (velocity != Vector3.zero) {
			rb.MovePosition (transform.position + velocity * Time.fixedTime);
		}
	}
}
