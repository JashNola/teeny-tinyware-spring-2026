using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Pigeon : MonoBehaviour
{
    public GameObject occupiedWirePoint;

    public enum PigeonStates // Becomes altered in the pigeonmanager script
    { 
        Flying,
        Waiting,
        Eating
    }

    public PigeonStates pigeonState; 

    public static event Action<int> onPigeonFed;
    public static event Action<int> onPigeonStarved;
    
    public int pigeonListIndex;
    public float lowerStarveRange;
    public float upperStarveRange;
    public bool isFed = false;

    private bool hasTriggeredExit = false;


    private void Start()
    {
        pigeonState = PigeonStates.Flying;
    }

    private void Update()
    {
        if (isFed && !hasTriggeredExit)
        {
            StartCoroutine(PigeonEatingLeaveDelay()); 
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PigeonRestingPoint"))
        {
            pigeonState = PigeonStates.Waiting;
            StartCoroutine(LeaveTimer());
        }

        else if (other.CompareTag("PigeonSpawnPoint") && this.gameObject.tag == "PigeonLeaving")
        {
            Destroy(this.gameObject); 
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
            pigeonState = PigeonStates.Flying; 
        }
    }

    IEnumerator PigeonEatingLeaveDelay()
    {
        yield return new WaitForSeconds(.5f);
        hasTriggeredExit = true;
        this.gameObject.tag = "PigeonLeaving";
        onPigeonFed?.Invoke(pigeonListIndex);
        pigeonState = PigeonStates.Flying; 

    }

}
