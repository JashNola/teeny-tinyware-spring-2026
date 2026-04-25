







using System.Collections;
using System.Collections.Generic; 
using NUnit.Framework;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;


    //private void Awake()
    //{
    //    instance = this; 
    //}

    //public List<Nodes> GeneratePath(Nodes start, Nodes end)
    //{
    //    List<Nodes> openSet = new List<Nodes>(); // List for path to find

    //    foreach(Nodes n in FindObjectsByType<Nodes>())
    //    {
    //        n.gScore = float.MaxValue; 
    //    }

    //    start.gScore = 0;
    //    start.hScore = Vector2.Distance(start.transform.position, end.transform.position);

    //    while (openSet.Count > 0)
    //    {
    //        int lowestF = default;

    //        for (int i = 1; i < openSet.Count; i++)
    //        {
    //            lowestF = i; 
    //        }

    //        Nodes currentNode = openSet[lowestF];
    //        openSet.Remove(currentNode); 

    //        if (currentNode == end) // If this is our end node
    //        {
    //            List<Nodes> path = new List<Nodes>();

    //            path.Insert(0, end); 

    //            while(currentNode != start) // Set current node to the value of of the came from node
    //            {
    //                path.Add(currentNode); 
    //            }
    //            path.Reverse();
    //            return path; 
    //        }

    //        foreach(Nodes connectedNode in currentNode.connections)
    //        {
    //            float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position); // MaKes sure the node we update has the most optimal values assigned to it
            
    //            if (heldGScore < connectedNode.gScore)
    //            {
    //                connectedNode.cameFrom = currentNode; 
    //                connectedNode.gScore = heldGScore;
    //                connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

    //                if (!openSet.Contains(connectedNode))
    //                {
    //                    openSet.Add(connectedNode); 
    //                }

    //            }
    //        }
    //    }
        
    //    return null; 
    
}
