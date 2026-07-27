using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ChonkCamera : MonoBehaviour
{
    #region Variables

    //tutorial : https://www.youtube.com/watch?v=q7hBQDpEY88

    [SerializeField] private Transform target;
    private float _distanceToPlayer;
    private Vector2 _input;

    [SerializeField] private MouseSensitivity mouseSensitivity;
    float originalYSens;
    [SerializeField] private CameraAngle cameraAngle;

    private CameraRotation _cameraRotation;

    public Slider sensitivitySlider;

    InventoryUI inventory;

    #endregion


    [SerializeField] private LayerMask cameraCollisionMask;
    [SerializeField] private float cameraRadius = 0.3f;
    //void Awake() => _distanceToPlayer = Vector3.Distance(transform.position, target.position);
    void Awake() {
        _distanceToPlayer = Vector3.Distance(transform.position, target.position);
        float linear = PlayerPrefs.GetFloat("cameraSensitivity");
        sensitivitySlider.value = linear;
    }
    public void SetSensitivity() {
        float linear = sensitivitySlider.value;
        PlayerPrefs.SetFloat("cameraSensitivity", linear);
        PlayerPrefs.Save();
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventory = FindObjectOfType<InventoryUI>();
        originalYSens = mouseSensitivity.vertical;
    }

    public void Look(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        mouseSensitivity.vertical = originalYSens;

        //controller settings ONLY
        if (context.control.device is Gamepad) {
            _input *= sensitivitySlider.value;
            mouseSensitivity.vertical = mouseSensitivity.horizontal * 1.1f;
        }
    }

    void Update()
    {
        if(!inventory.inventoryUI.activeSelf)
        {
            //get mouse input only when inventory is closed
            _cameraRotation.Yaw += _input.x * mouseSensitivity.horizontal * BoolToInt(mouseSensitivity.invertHorizontal) * Time.deltaTime;
            _cameraRotation.Pitch += _input.y * mouseSensitivity.vertical * BoolToInt(mouseSensitivity.invertVertical) * Time.deltaTime;
            _cameraRotation.Pitch = Mathf.Clamp(_cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);
        }
    }

    /*void LateUpdate()
    {
        transform.eulerAngles = new Vector3(_cameraRotation.Pitch, _cameraRotation.Yaw, 0.0f);
        transform.position = target.position - transform.forward * _distanceToPlayer;
    }*/
    void LateUpdate() {
        Quaternion rotation = Quaternion.Euler(_cameraRotation.Pitch, _cameraRotation.Yaw, 0f);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * _distanceToPlayer;

        if (Physics.SphereCast(target.position, cameraRadius, desiredPosition - target.position,
            out RaycastHit hit, _distanceToPlayer, cameraCollisionMask)) {
            transform.position = hit.point + hit.normal * cameraRadius;
        } else {
            transform.position = desiredPosition;
        }

        transform.rotation = rotation;
    }

    private static int BoolToInt(bool b) => b ? 1 : -1;
}

[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
    public bool invertHorizontal;
    public bool invertVertical;
}

public struct CameraRotation
{
    public float Pitch;
    public float Yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min;
    public float max;
}