using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    // workaround
    private static GameObject player;
    public static void SetPlayer(GameObject p)
    {
        if (player == null)
            player = p;
    }
    public static float GetPlayerPositionX() {
        return player != null ? player.transform.position.x : 0;
    }

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
