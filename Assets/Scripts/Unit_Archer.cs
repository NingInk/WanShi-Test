using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Archer : Unit_Base
{
    GameObject Bullet;
    protected override void Awake()
    {
        base.Awake();
        Bullet = GetComponentInChildren<Weapon_Base>().gameObject;
        Bullet.SetActive(false);
    }
    protected override void Attack()
    {
        GameObject gg = Instantiate(Bullet, Bullet.transform.position, transform.rotation);
        gg.SetActive(true);
        gg.transform.LookAt(target.transform);
    }
}
