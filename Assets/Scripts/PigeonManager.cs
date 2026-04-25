using System;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[Serializable]
public class PigeonData
{
    public GameObject pigeonObject;
    public GameObject pidgeonSpawnPoints;
    public GameObject pidgeonWirePoints;
    public int inputIndex;
}

public class PigeonManager : MonoBehaviour
{
    public GameObject pigeonPrefab;
    public PigeonData[] pigeonArray;
    public List<GameObject> spawnPoints;
    public List<GameObject> wirePoints;

    GameObject pigeonContainer;
    public float PigeonLeaveDelay;

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

    private void Start()
    {
        pigeonContainer = transform.GetChild(0).gameObject;

        pigeonArray = new PigeonData[wirePoints.Count];
        for (int i = 0; i < pigeonArray.Length; i++)
        {
            pigeonArray[i] = new PigeonData();
        }

        SetPigeonEnter();
    }

    void SetPigeonEnter()
    {
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

        if (spawnPoints.Count == 0) return;
        int randomSpawn = UnityEngine.Random.Range(0, spawnPoints.Count);
        GameObject selectedSpawn = spawnPoints[randomSpawn];

        List<GameObject> availableWires = new List<GameObject>();

        foreach (GameObject w in wirePoints)
        {
            if (w == null) continue;

            WirePoint wp = w.GetComponent<WirePoint>();
            if (wp != null)
            {
                if (!wp.isOccupied)
                {
                    availableWires.Add(w);
                }
            }
            else
            {
                Debug.LogWarning("Object " + w.name + " is in wirePoints list but is missing the WirePoint component!");
            }
        }

        if (availableWires.Count > 0)
        {
            GameObject selectedWire = availableWires[UnityEngine.Random.Range(0, availableWires.Count)];

            if (selectedSpawn == null) return;

            GameObject newPigeon = Instantiate(pigeonPrefab, selectedSpawn.transform.position, selectedSpawn.transform.rotation);
            newPigeon.transform.SetParent(pigeonContainer.transform);

            pigeonArray[slotIndex].pigeonObject = newPigeon;
            pigeonArray[slotIndex].pidgeonSpawnPoints = selectedSpawn;
            pigeonArray[slotIndex].pidgeonWirePoints = selectedWire;
            pigeonArray[slotIndex].inputIndex = slotIndex;

            newPigeon.GetComponent<Pigeon>().pigeonListIndex = slotIndex;

            newPigeon.GetComponent<AIDestinationSetter>().target = selectedWire.transform;
            selectedWire.GetComponent<WirePoint>().isOccupied = true;
        }
    }

    private void SetPigeonFed(int obj)
    {
        if (obj < 0 || obj >= pigeonArray.Length || pigeonArray[obj].pigeonObject == null) return;

        if (pigeonArray[obj].pidgeonWirePoints != null)
        {
            WirePoint wp = pigeonArray[obj].pidgeonWirePoints.GetComponent<WirePoint>();
            if (wp != null) wp.isOccupied = false;
        }

        StartCoroutine(PigeonLeaveCoroutine());

        int randomExit = UnityEngine.Random.Range(0, spawnPoints.Count);
        if (spawnPoints[randomExit] != null)
            pigeonArray[obj].pigeonObject.GetComponent<AIDestinationSetter>().target = spawnPoints[randomExit].transform;

        pigeonArray[obj].pigeonObject = null;
    }

    private void SetPigeonStarved(int obj)
    {
        if (obj < 0 || obj >= pigeonArray.Length || pigeonArray[obj].pigeonObject == null) return;

        if (pigeonArray[obj].pidgeonWirePoints != null)
        {
            WirePoint wp = pigeonArray[obj].pidgeonWirePoints.GetComponent<WirePoint>();
            if (wp != null) wp.isOccupied = false;
        }

        int randomExit = UnityEngine.Random.Range(0, spawnPoints.Count);
        if (spawnPoints[randomExit] != null)
            pigeonArray[obj].pigeonObject.GetComponent<AIDestinationSetter>().target = spawnPoints[randomExit].transform;

        pigeonArray[obj].pigeonObject = null;
    }

    IEnumerator PigeonLeaveCoroutine()
    {
        yield return new WaitForSeconds(PigeonLeaveDelay);
    }
}
