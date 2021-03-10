using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitGroup : MonoBehaviour
{
    public GameObject[] units;
    public GameObject unit_ex;
    private void OnEnable()
    {
        Controller.group = this;

        for (int i = 0; i < units.Length; i++)
        {
            GameObject gg = Instantiate(unit_ex, transform);
            gg.GetComponentInChildren<Text>().text = units[i].GetComponent<Unit_Base>()._name;
            gg.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
            gg.GetComponent<Toggle>().onValueChanged.AddListener((bool arg0) => { Controller.current = gg.GetComponent<Toggle>(); });
        }
    }

    private void OnDisable()
    {
        if (Controller.group = this)
        {
            Controller.group = null;
        }
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject GetGameObject()
    {
        if (Controller.current)
        {
            foreach (var item in units)
            {
                if (item.GetComponent<Unit_Base>()._name == Controller.current.GetComponentInChildren<Text>().text)
                {
                    item.tag = transform.tag;
                    return item;
                }
            }
        }
        return null;
    }
}
