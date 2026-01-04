using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	public Transform target;
	public Vector3 defaultDistance = new Vector3(0f, 2f, -10f);
	public float distanceDamp = 10f;
	//public float rotationalDamp = 10f;
	public Vector3 velocity = Vector3.one;

	Transform player;

	void LateUpdate () {
		SmoothFollow ();
		/*Vector3 toPos = target.position + (target.rotation * defaultDistance);
		Vector3 curPos = Vector3.Lerp (player.position, toPos, distanceDamp * Time.deltaTime);
		player.position = curPos;

		Quaternion toRot = Quaternion.LookRotation (target.position - player.position, target.up);
		Quaternion curRot = Quaternion.Slerp (player.rotation, toRot, rotationalDamp * Time.deltaTime);
		player.rotation = curRot;*/
	}
	void SmoothFollow() {
		Vector3 toPos = target.position + (target.rotation * defaultDistance);
		Vector3 curPos = Vector3.SmoothDamp (transform.position, toPos, ref velocity, distanceDamp);
        transform.position = curPos;
        transform.LookAt (target, target.up);
	}
}