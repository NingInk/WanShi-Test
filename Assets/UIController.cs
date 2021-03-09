using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static Toggle current;
    public static UnitGroup group;

    public static bool isPlaying;

    public Toggle[] togs;
    public RectTransform[] tras;


    public void StartGame()
    {
        isPlaying = true;
    }

}
