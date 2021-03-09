using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject master;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == master.tag)
            return;
        if (other.GetComponent<BaseUnit>())
        {
            Debug.Log(other.name);
            other.GetComponent<BaseUnit>().SetBlood(master.GetComponent<BaseUnit>().aggressivity, master);
        }
    }
}
