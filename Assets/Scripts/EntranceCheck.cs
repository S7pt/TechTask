using System;
using UnityEngine;

public class EntranceCheck : MonoBehaviour
{
    private static event Action passedDoors;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            passedDoors();
        }
    }

    public static void SubscribeToPassedDoors(Action method)
    {
        passedDoors += method;
    }
}
