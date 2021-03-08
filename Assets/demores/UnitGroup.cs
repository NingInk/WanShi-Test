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
        UIController.group = this;

        for (int i = 0; i < units.Length; i++)
        {
            GameObject gg = Instantiate(unit_ex, transform);
            gg.GetComponentInChildren<Text>().text = units[i].GetComponent<BaseUnit>()._name;
            gg.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
            gg.GetComponent<Toggle>().onValueChanged.AddListener((bool arg0) => { UIController.current = gg.GetComponent<Toggle>(); });
        }
    }

    private void OnDisable()
    {
        if (UIController.group = this)
        {
            UIController.group = null;
        }
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject GetGameObject()
    {
        if (UIController.current)
        {
            foreach (var item in units)
            {
                if (item.GetComponent<BaseUnit>()._name == UIController.current.GetComponentInChildren<Text>().text)
                {
                    return item;
                }
            }
        }
        return null;
    }
}
