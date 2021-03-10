using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : Weapon_Base
{
    Vector3 pos;
    Quaternion rot;
    private void Awake()
    {
        pos = transform.localPosition;
        rot = transform.localRotation;
    }
    private void FixedUpdate()
    {
        transform.localPosition = pos;
        transform.localRotation = rot;
    }
}
