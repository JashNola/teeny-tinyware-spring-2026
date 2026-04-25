using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pigeon : MonoBehaviour
{
    public int pigeonListIndex;
    public bool isFed = false;

    public static event Action<int> onPigeonFed;
    private void Update()
    {
        if (isFed)
        {
            onPigeonFed?.Invoke(pigeonListIndex);
        }
    }





}
