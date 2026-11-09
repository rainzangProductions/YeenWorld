using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomIn : MonoBehaviour
{
    float defaultPOV;
    public float scopedPOV = 30;
    Camera mainCamera;
    bool justUnscoped = false;

    //lerp stuff with this tutorial: https://www.youtube.com/watch?v=RNccTrsgO9g
    float lerpedValue;
    public float scopeDuration = 0.0035f;
    float targetIn;
    float targetOut;
    
    //you can delete totalScale but I'm keeping it in case I need this code for future scripts
    float totalScale = 1;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        defaultPOV = mainCamera.fieldOfView;
        targetIn = scopedPOV;
        targetOut = mainCamera.fieldOfView;
    }
    void Update()
    {
        //targetIn = scopedPOV;
        if(Input.GetButton("Scope In"))
        {
            lerpedValue = Mathf.MoveTowards(lerpedValue, targetIn, (totalScale / scopeDuration) * Time.deltaTime);
            //Debug.Log(lerpedValue.ToString());
            mainCamera.fieldOfView = lerpedValue;

            //Look in camera direction
            Vector3 direction = mainCamera.transform.forward;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Input.GetButtonUp("Scope In"))
        {
            justUnscoped = true;
        }

        if (!Input.GetButton("Scope In") && justUnscoped)
        {
            lerpedValue = Mathf.MoveTowards(lerpedValue, targetOut, (totalScale / scopeDuration) * Time.deltaTime);
            //Debug.Log(lerpedValue.ToString());
            mainCamera.fieldOfView = lerpedValue;
        }

        if(mainCamera.fieldOfView >= defaultPOV) { justUnscoped = false; mainCamera.fieldOfView = defaultPOV; }
    }

}
