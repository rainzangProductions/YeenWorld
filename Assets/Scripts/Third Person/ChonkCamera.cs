using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChonkCamera : MonoBehaviour
{
    #region Variables

    //tutorial : https://www.youtube.com/watch?v=q7hBQDpEY88

    [SerializeField] private Transform target;
    private float _distanceToPlayer;
    private Vector2 _input;

    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private CameraAngle cameraAngle;

    private CameraRotation _cameraRotation;

    InventoryUI inventory;

    #endregion

    void Awake() => _distanceToPlayer = Vector3.Distance(transform.position, target.position);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventory = FindObjectOfType<InventoryUI>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
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

    void LateUpdate()
    {
        transform.eulerAngles = new Vector3(_cameraRotation.Pitch, _cameraRotation.Yaw, 0.0f);
        transform.position = target.position - transform.forward * _distanceToPlayer;
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