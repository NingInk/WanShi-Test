using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float scaleSpeed = 1;
    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float mou = Input.GetAxis("Mouse ScrollWheel");

        Vector3 move = new Vector3(hor, 0, ver) * Time.deltaTime * moveSpeed;
        transform.position += move;
        transform.position += transform.forward * mou * Time.deltaTime * scaleSpeed;
    }
}
