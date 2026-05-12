using Pathfinding;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class StartEndController : MonoBehaviour
{
    [Header("Object References")]


    public GameObject clock;
    public GameObject startScreen;
    public GameObject endScreen;

    [SerializeField] private Sprite[] endScreenOptions; // 0 = win, 1 = lose
    public TMP_Text endTitle;
    public TMP_Text endSubtitle;

    public float endTextDelay = 1f;
    private bool gameActive = false;
    private bool firstGame = true;
    void Start()
    {
        gameActive = false;
    }

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!gameActive && Input.GetKeyDown((KeyCode)(48 + i)))
            {
                if (firstGame) firstGame = false;
                StartGame();
            }
        }
    }

    void StartGame()
    {
        if (!PigeonManager.instance.canSpawn)
        {
            PigeonManager.instance.canSpawn = true;
            PigeonManager.instance.StartCoroutine(PigeonManager.instance.SpawnRoutine());
        }

        Debug.Log("Game Started");

        gameActive = true;

        startScreen.SetActive(false);

        endScreen.SetActive(false);

        GameplayController.instance.totalPigeonsFed = 0;

        clock.SetActive(true);
    }

    public void EndGame()
    {
        PigeonManager.instance.canSpawn = false;

        for (int i = 0; i < PigeonManager.instance.pigeonArray.Length; i++)
        {
            if (PigeonManager.instance.pigeonArray[i].pigeonObject != null)
            {
                Destroy(PigeonManager.instance.pigeonArray[i].pigeonObject);

                PigeonManager.instance.pigeonArray[i].pigeonWirePoints = null;
                PigeonManager.instance.pigeonArray[i].pigeonSpawnPoints = null;
                PigeonManager.instance.pigeonArray[i].pigeonObject = null;
            }
        }

        EndingCheck();

        GameplayController.instance.maxPigeonCount = 1; // Setting pigeon spawn limit to one at a time

        PigeonManager.instance.spawnCheckInterval = 3f; // Setting pigeons back to their default spawning speed

        clock.SetActive(false);

        endScreen.SetActive(true);

        StartCoroutine("EndingAnimations");
    }

    IEnumerator EndingAnimations()
    {
        // fade in text sequentially
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(endTextDelay);

            endScreen.transform.GetChild(i).gameObject.SetActive(true);
        }

        gameActive = false;

        yield break;
    }

    private void EndingCheck()
    {
        int lastIndex = GameplayController.instance.difficultySettings.levels.Length - 1;
        int finalDifficultyMax = GameplayController.instance.difficultySettings.levels[lastIndex].difficultyMaxPigeonCounts;

        if (GameplayController.instance.maxPigeonCount == finalDifficultyMax)
        {
            endScreen.GetComponent<Image>().sprite = endScreenOptions[0];
            endTitle.text = "pigeon takeover >:P";
            endSubtitle.text = "play again";
        }
        else
        {
            endScreen.GetComponent<Image>().sprite = endScreenOptions[1];
            endTitle.text = "pigeon starve :(";
            endSubtitle.text = "try again";
        }


    }
}