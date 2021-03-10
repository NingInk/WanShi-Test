using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bullet : Weapon_Base
{
    public float speed = 1;

    float y;
    private void OnEnable()
    {
        y = transform.position.y;
    }
    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
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
