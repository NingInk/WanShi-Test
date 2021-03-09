using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeBehaviour : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if (UIController.current)
        {
            GetComponent<Renderer>().material.color = UIController.current.graphic.color;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    bool have = false;
    private void OnMouseDown()
    {
        if (have)
            return;
        have = true;
        if (UIController.group)
        {
            GameObject gg = UIController.group.GetGameObject();
            if (gg != null)
            {
                gg = Instantiate(gg, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                if (gg.tag == "Red")
                {
                    gg.transform.SetParent(UIController.Instance.redBornAt.transform);
                }
                if (gg.tag == "Blue")
                {
                    gg.transform.SetParent(UIController.Instance.BlueBornAt.transform);
                }
                gg.transform.eulerAngles = Vector3.up * Random.Range(0f, 360f);
            }
        }

    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
