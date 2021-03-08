using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniController : MonoBehaviour
{
    Animation ani;
    private void Awake()
    {
        ani = GetComponent<Animation>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("idle"))
        {
            ani.Play("idle@loop");
        }
        if (GUILayout.Button("walk"))
        {
            ani.Play("walk@loop");
        }
        if (GUILayout.Button("run"))
        {
            ani.Play("run@loop");
        }
        if (GUILayout.Button("damage"))
        {
            ani.Play("damage");
        }
        if (GUILayout.Button("die"))
        {
            ani.Play("die");
        }
    }

    public void SetAni(string aniName)
    {
        ani.Play(aniName);
    }
}

public static class Ani
{
    public static string idle = "idle@loop";
    public static string walk = "walk@loop";
    public static string run = "run@loop";
    public static string danmage = "danmage";
    public static string die = "die";
    public static string attack1 = "attack_sword_01";
    public static string attack2 = "attack_sword_02";
    public static string attack3 = "attack_sword_03";
    public static string jump = "jump";
    public static string sit = "sit";
    public static string sit_idle = "sit_idle";
    public static string stand = "stand";
    public static string tumbling = "tumbling";

}