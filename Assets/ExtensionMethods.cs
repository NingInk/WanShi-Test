using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// 保证目标具有 T 脚本并获取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Transform trans) where T : Component
    {
        return trans.gameObject.GetComponent<T>() == null ?
             trans.gameObject.AddComponent<T>() :
             trans.gameObject.GetComponent<T>();
    }
    /// <summary>
    /// 保证目标具有 T 脚本并获取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gg"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject gg) where T : Component
    {
        return gg.gameObject.GetComponent<T>() == null ?
             gg.gameObject.AddComponent<T>() :
             gg.gameObject.GetComponent<T>();
    }
    /// <summary>
    /// 移除 T 组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    public static void RemoveComponent<T>(this GameObject go) where T : Component
    {
        if (go.GetComponent<T>() != null)
        {
            Component.Destroy(go.GetComponent<T>());
        }
    }

    /// <summary>
    /// 克隆自己并设置为目标子对象
    /// </summary>
    /// <param name="gg">自己</param>
    /// <param name="trans">目标</param>
    /// <returns></returns>
    public static GameObject CopyMyself(this GameObject gg, Transform trans)
    {
        return GameObject.Instantiate<GameObject>(gg, trans);
    }

    /// <summary>
    /// 获取路径最后的名字（不包括后缀）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetPathLastName(this string path, char c = '\\')
    {
        return path.Split(c)[path.Split(c).Length - 1].Split('.')[0];
    }

    /// <summary>
    /// 最后一个子对象
    /// </summary>
    /// <param name="tran"></param>
    /// <returns></returns>
    public static Transform LastChild(this Transform tran)
    {
        return tran.GetChild(tran.childCount - 1);
    }

    /// <summary>
    /// 添加富文本标记（颜色）
    /// </summary>
    /// <param name="str"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string Color(this string str, string col = "green")
    {
        return "<color=" + col + ">" + str + "</color>";
    }
}
