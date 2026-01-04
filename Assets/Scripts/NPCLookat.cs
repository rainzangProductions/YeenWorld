using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookat : MonoBehaviour {

	public float rotSpeed = 3f;
    public bool OnlyLookAtPlayer;
	public bool isInanimate;

	public void LookAt(Transform player) {
		Vector3 rotTarget = player.position - transform.position;
		rotTarget.y = 0;
		Quaternion rotTarget2 = Quaternion.LookRotation (rotTarget);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget2, rotSpeed * Time.deltaTime);
		//Debug.Log ("Rotated");
	}
}