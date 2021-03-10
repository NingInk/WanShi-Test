using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : BaseManager_Mono<Controller>
{
    public static Toggle current;
    public static UnitGroup group;

    public static GameObject[] floors;

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

    public void RestartGame()
    {
        IsPlaying = false;
        foreach (Transform child in redBornAt.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in BlueBornAt.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Awake()
    {
        floors = GameObject.FindGameObjectsWithTag("Floor");
    }

    private void Update()
    {
        return;
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
