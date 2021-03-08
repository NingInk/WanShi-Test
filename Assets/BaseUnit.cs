using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BaseUnit : MonoBehaviour
{
    public string _name;
    public int bloodVolume = 10;
    public int aggressivity = 2;
    public float CD = 1;
    public int speed = 1;
    //public int bloodVolume;

    protected virtual void Attack()
    {

    }

    protected virtual void FollowUp()
    {

    }
}
