using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScene : MonoBehaviour
{
    public GameObject prefab;
    public int index = 8;

    private void Awake()
    {
        index += 2;
        for (int i = -index / 2; i < index / 2; i++)
        {
            for (int j = -index / 2; j < index / 2; j++)
            {
                GameObject go = Instantiate(prefab, new Vector3(0.5f + i, -0.05f, 0.5f + j), Quaternion.identity);
                go.transform.SetParent(transform);
                if (i == (-index / 2) || i == (index / 2) - 1 || j == (-index / 2) || j == (index / 2) - 1)
                {
                    go.transform.localScale = Vector3.one;
                }
                else
                {
                    go.AddComponent<CubeBehaviour>();
                }
            }
        }
    }
}
