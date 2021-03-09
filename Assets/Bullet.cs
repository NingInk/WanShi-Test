using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    public float speed = 1;
    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        //transform.LookAt(master.GetComponent<BaseUnit>().target.transform);
        transform.position += transform.forward * Time.deltaTime * speed;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == (master.tag == "Red" ? "BLue" : "Red") || other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
