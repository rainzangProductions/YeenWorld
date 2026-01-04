using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRotation : MonoBehaviour
{
    [Header("Speed per Axis")]
    public int x;
    public int y;
    public int z;
    void Update()
    {
        transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }
}