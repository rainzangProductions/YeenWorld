using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomIn : MonoBehaviour
{
    float defaultPOV;
    float scopedPOV = 25;
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
        if(Input.GetButton("Scope In"))
        {
            lerpedValue = Mathf.MoveTowards(lerpedValue, targetIn, (totalScale / scopeDuration) * Time.deltaTime);
            //Debug.Log(lerpedValue.ToString());
            mainCamera.fieldOfView = lerpedValue;
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
