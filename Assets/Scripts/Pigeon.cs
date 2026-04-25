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

    private void Update()
    {
        // Checks whether a pigeon has been fed
        if (isFed)
        {
            onPigeonFed?.Invoke(pigeonListIndex);
            this.gameObject.tag = "PigeonLeaving"; 
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
        this.gameObject.tag = "PigeonLeaving";
        onPigeonStarved?.Invoke(pigeonListIndex);
    }



}
