using System;
using Pathfinding; 
using System.Collections.Generic; 
using UnityEngine;

public class PigeonManager : MonoBehaviour
{
    public GameObject pigeonPrefab; 
    public List<GameObject> pigeonsPresent;
    public List<GameObject> pigeonSpawnPoints;
    public List<GameObject> pigeonWirePoint;

    GameObject pigeonContainer;

    private void OnEnable()
    {
        Pigeon.onPigeonFed += SetPigeonLeave;
    }

    private void OnDisable()
    {
        Pigeon.onPigeonFed -= SetPigeonLeave;
    }

    private void Start()
    {
        pigeonContainer = transform.GetChild(0).gameObject;
        SetPigeonEnter(); 
    }


    void SetPigeonEnter()
    {
        // Spawns a pigeon 
        int spawnPointIndex = UnityEngine.Random.Range(0, pigeonSpawnPoints.Count);

        GameObject newPigeon = Instantiate(pigeonPrefab, pigeonSpawnPoints[spawnPointIndex].transform.position, pigeonSpawnPoints[spawnPointIndex].transform.rotation);
        newPigeon.transform.SetParent(pigeonContainer.transform); // Stuffs newly spawned pigeon in our empty gameobject for 'em
        pigeonsPresent.Add(newPigeon);

        for (int i = 0; i < pigeonsPresent.Count; i++)
        {
            if (pigeonsPresent[i] == newPigeon)
            {
                newPigeon.GetComponent<Pigeon>().pigeonListIndex = i; 
            }
        }

        // Set a pigeons randomized wire point
        int wirePointIndex = UnityEngine.Random.Range(0, pigeonWirePoint.Count);
        newPigeon.GetComponent<AIDestinationSetter>().target = pigeonWirePoint[wirePointIndex].transform; 
    }

    private void SetPigeonLeave(int obj)
    {
        // Making a pigeon fly away </3
        int spawnPointIndex = UnityEngine.Random.Range(0, pigeonSpawnPoints.Count);


        pigeonsPresent[obj].GetComponent<AIDestinationSetter>().target = pigeonSpawnPoints[spawnPointIndex].transform;  
    }



}
