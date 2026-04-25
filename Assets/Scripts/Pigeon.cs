using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Pigeon : MonoBehaviour
{
    public GameObject occupiedWirePoint;

    public static event Action<int> onPigeonFed;
    public static event Action<int> onPigeonStarved;
    
    public int pigeonListIndex;
    public float lowerStarveRange;
    public float upperStarveRange;
    public bool isFed = false;

    private bool hasTriggeredExit = false; // New flag to prevent repeat firing

    private void Update()
    {
        if (isFed && !hasTriggeredExit)
        {
            hasTriggeredExit = true; 
            this.gameObject.tag = "PigeonLeaving";
            onPigeonFed?.Invoke(pigeonListIndex);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PigeonRestingPoint"))
        {
            StartCoroutine(LeaveTimer());
        }

        else if (other.CompareTag("PigeonSpawnPoint") && this.gameObject.tag == "PigeonLeaving")
        {
            Debug.Log("Pigeon will be destroyed now");
            Destroy(this.gameObject); 
        }

        else
        {
            Debug.Log(other.tag);
            Debug.Log(this.tag);
        }
    }

    IEnumerator LeaveTimer()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(lowerStarveRange, upperStarveRange));

        if (!hasTriggeredExit)
        {
            hasTriggeredExit = true;
            this.gameObject.tag = "PigeonLeaving";
            onPigeonStarved?.Invoke(pigeonListIndex);
        }
    }



}
