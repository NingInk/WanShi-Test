using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static Toggle current;
    public static UnitGroup group;

    public Toggle[] togs;
    public RectTransform[] tras;

    public UIController ins;
    private void Awake()
    {
        ins = this;
    }



}
