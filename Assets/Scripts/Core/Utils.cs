using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static GameObject PullInactiveGameObjectFromList(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (!list[i].activeInHierarchy)
                return list[i];
        }
        return list[0];
    }
}
