using System;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[Serializable]
public class PigeonData
{
    public GameObject pigeonObject;
    public GameObject pigeonSpawnPoints;
    public GameObject pigeonWirePoints;
    public int inputIndex;
}

public class PigeonManager : MonoBehaviour
{

    public static PigeonManager instance;

    public float spawnCheckInterval = .5f;
    public float pigeonLeaveDelay = 3.0f;

    [Header("Settings")]
    public GameObject pigeonPrefab;


    [Header("References")]
    public PigeonData[] pigeonArray;
    public List<GameObject> spawnPoints;
    public List<GameObject> wirePoints;

    private GameObject pigeonContainer;

    private void OnEnable()
    {
        Pigeon.onPigeonFed += SetPigeonFed;
        Pigeon.onPigeonStarved += SetPigeonStarved;
    }

    private void OnDisable()
    {
        Pigeon.onPigeonFed -= SetPigeonFed;
        Pigeon.onPigeonStarved -= SetPigeonStarved;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pigeonContainer = transform.GetChild(0).gameObject;

        pigeonArray = new PigeonData[wirePoints.Count];
        for (int i = 0; i < pigeonArray.Length; i++)
        {
            pigeonArray[i] = new PigeonData();
        }

        // Start the spawning cycle
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // how many pigeons we got?
            int activeCount = 0;
            for (int i = 0; i < pigeonArray.Length; i++)
            {
                if (pigeonArray[i].pigeonObject != null) activeCount++;
            }

            // Checking the difficulty limit
            if (activeCount < GameplayController.instance.maxPigeonCount)
            {
                SetPigeonEnter();
            }

            yield return new WaitForSeconds(spawnCheckInterval);
        }
    }

    void SetPigeonEnter()
    {
        // Find first empty slot in the data array
        int slotIndex = -1;
        for (int i = 0; i < pigeonArray.Length; i++)
        {
            if (pigeonArray[i].pigeonObject == null)
            {
                slotIndex = i;
                break;
            }
        }

        if (slotIndex == -1) return;

        List<int> availableWireIndices = new List<int>();
        for (int i = 0; i < wirePoints.Count; i++)
        {
            if (wirePoints[i] != null)
            {
                WirePoint wp = wirePoints[i].GetComponent<WirePoint>();
                if (wp != null && !wp.isOccupied) availableWireIndices.Add(i);
            }
        }

        if (availableWireIndices.Count > 0 && spawnPoints.Count > 0)
        {
            // Pick random wire and spawn point
            int chosenWireIdx = availableWireIndices[UnityEngine.Random.Range(0, availableWireIndices.Count)];
            GameObject selectedWire = wirePoints[chosenWireIdx];
            GameObject selectedSpawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

            // Instantiate
            GameObject newPigeon = Instantiate(pigeonPrefab, selectedSpawn.transform.position, selectedSpawn.transform.rotation);
            newPigeon.transform.SetParent(pigeonContainer.transform);

            
            pigeonArray[slotIndex].pigeonObject = newPigeon;
            pigeonArray[slotIndex].pigeonSpawnPoints = selectedSpawn;
            pigeonArray[slotIndex].pigeonWirePoints = selectedWire;
            pigeonArray[slotIndex].inputIndex = chosenWireIdx + 1;

            // pathfinding
            newPigeon.GetComponent<Pigeon>().pigeonListIndex = slotIndex;
            newPigeon.GetComponent<AIDestinationSetter>().target = selectedWire.transform;

            selectedWire.GetComponent<WirePoint>().isOccupied = true;
        }
    }

    private void SetPigeonFed(int obj)
    {
        ProcessPigeonExit(obj);
    }

    private void SetPigeonStarved(int obj)
    {
        ProcessPigeonExit(obj);
    }

    private void ProcessPigeonExit(int index)
    {
        if (index < 0 || index >= pigeonArray.Length || pigeonArray[index].pigeonObject == null) return;

        if (pigeonArray[index].pigeonWirePoints != null)
        {
            pigeonArray[index].pigeonWirePoints.GetComponent<WirePoint>().isOccupied = false;
        }

        // Flying away </3
        int randomExit = UnityEngine.Random.Range(0, spawnPoints.Count);
        pigeonArray[index].pigeonObject.GetComponent<AIDestinationSetter>().target = spawnPoints[randomExit].transform;

        // Cleanup
        StartCoroutine(PigeonLeaveCoroutine(pigeonArray[index].pigeonObject));
        pigeonArray[index].pigeonObject = null;
    }

    IEnumerator PigeonLeaveCoroutine(GameObject pigeon)
    {
        yield return new WaitForSeconds(pigeonLeaveDelay);
    }
}