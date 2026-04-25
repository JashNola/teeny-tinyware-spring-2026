using System;
using UnityEngine;

[Serializable]
public class DifficultyLevels
{
    // These must be public to be accessed by the loop
    public int criteria;
    public int difficultyMaxPigeonCounts;
}

[Serializable]
public class DifficultySettings
{
    public DifficultyLevels[] levels;
}

public class GameplayController : MonoBehaviour
{
    public DifficultySettings difficultySettings;

    public static GameplayController instance;
    public int maxPigeonCount = 1;
    public int totalPigeonsFed = 0;

    private void OnEnable()
    {
        Pigeon.onPigeonFed += PigeonFedTracking;
    }

    private void OnDisable()
    {
        Pigeon.onPigeonFed -= PigeonFedTracking;
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

    private void PigeonFedTracking(int pigeonIndex)
    {
        totalPigeonsFed++;
        DifficultyTracking();
    }

    private void DifficultyTracking()
    {
        for (int i = 0; i < difficultySettings.levels.Length; i++)
        {
            if (totalPigeonsFed >= difficultySettings.levels[i].criteria)
            {
                // Update the global max count
                maxPigeonCount = difficultySettings.levels[i].difficultyMaxPigeonCounts;
            }
        }
    }
}