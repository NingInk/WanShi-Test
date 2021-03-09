using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : BaseManager_Mono<UIController>
{
    public static Toggle current;
    public static UnitGroup group;

    private static bool isPlaying;
    public static System.Action playAct;


    public Toggle[] togs;
    public RectTransform[] tras;

    public GameObject redBornAt, BlueBornAt;

    public static bool IsPlaying
    {
        get => isPlaying;
        set
        {
            isPlaying = value;
            playAct?.Invoke();
        }
    }

    public void StartGame()
    {
        IsPlaying = true;
    }
    private void Update()
    {
        if (IsPlaying)
        {
            if (redBornAt.transform.childCount == 0 || BlueBornAt.transform.childCount == 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }
    }
}
