using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookat : MonoBehaviour {

	public bool freezeX = true;
	void Update () {
		if (freezeX) {
			transform.rotation = Quaternion.Euler (0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
		} else {
			transform.rotation = Camera.main.transform.rotation;
		}
	}
}