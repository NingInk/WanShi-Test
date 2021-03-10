using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITest : MonoBehaviour
{
    NavMeshAgent nav;
    private RaycastHit hit;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    Vector3 target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.point;
                nav.destination = hit.point;
                nav.speed = 0.5f;
            }
        }


        if (Vector3.Angle(transform.forward, target - transform.position) < 30)
        {
            nav.speed = 3.5f;
        }
    }

    private void OnGUI()
    {
        GUILayout.TextField(Vector3.Angle(transform.forward, target - transform.position).ToString());
    }
}
