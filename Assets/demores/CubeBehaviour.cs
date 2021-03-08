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
            Debug.Log(UIController.current.GetComponentInChildren<Text>().text);
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }
    private void OnMouseDown()
    {
        if (UIController.group)
        {
            GameObject gg = UIController.group.GetGameObject();
            if (gg != null)
            {
                gg = Instantiate(gg, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            }
        }
        
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
