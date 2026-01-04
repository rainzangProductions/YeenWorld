using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public float sensX;
	public float sensY;
	public Transform orientation;
	float rotX;
	float rotY;
	float clamp = 90f;

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		float mouseX = Input.GetAxisRaw ("Mouse X") * Time.deltaTime * sensX;
		float mouseY = Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * sensY;
		rotY += mouseX;

		rotX -= mouseY;
		rotX = Mathf.Clamp (rotX, -clamp, clamp);

		transform.rotation = Quaternion.Euler (rotX, rotY, 0);
		orientation.rotation = Quaternion.Euler (0, rotY, 0);

	}
}