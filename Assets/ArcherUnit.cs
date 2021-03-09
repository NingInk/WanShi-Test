using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUnit : BaseUnit
{
    GameObject Bullet;
    protected override void Awake()
    {
        base.Awake();
        Bullet = GetComponentInChildren<Weapon>().gameObject;
        Bullet.SetActive(false);
    }
    protected override void Attack()
    {
        GameObject gg = Instantiate(Bullet, Bullet.transform.position, transform.rotation);
        gg.SetActive(true);
        gg.transform.LookAt(target.transform);
    }
}
