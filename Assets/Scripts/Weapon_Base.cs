using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Base : MonoBehaviour
{
    public GameObject master;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (master == null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.tag == master.tag)
            return;
        if (other.GetComponent<Unit_Base>())
        {
            Debug.Log(other.name);
            other.GetComponent<Unit_Base>().SetBlood(master.GetComponent<Unit_Base>().aggressivity, master);
        }
    }
}
