using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeBehaviour : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if (Controller.current)
        {
            GetComponent<Renderer>().material.color = Controller.current.graphic.color;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    bool have = false;
    private void OnMouseDown()
    {
        if (have && Controller.IsPlaying)
            return;
        have = true;
        if (Controller.group)
        {
            GameObject gg = Controller.group.GetGameObject();
            if (gg != null)
            {
                gg = Instantiate(gg, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                if (gg.tag == "Red")
                {
                    gg.transform.SetParent(Controller.Instance.redBornAt.transform);
                    gg.GetComponentInChildren<Image>().color = Color.red;
                }
                if (gg.tag == "Blue")
                {
                    gg.transform.SetParent(Controller.Instance.BlueBornAt.transform);
                    gg.GetComponentInChildren<Image>().color = Color.blue;
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
