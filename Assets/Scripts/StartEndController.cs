using System.Collections;
using UnityEngine;

public class StartEndController : MonoBehaviour
{
    [Header("Object References")]
    public GameObject clock;
    public GameObject startScreen;
    public GameObject endScreen;

    public float endTextDelay = 0.5f;
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
        Debug.Log("Game Started");

        gameActive = true;

        startScreen.SetActive(false);
        endScreen.SetActive(false);

        /*
        for (int i = 0; i < 3; i++)
        {
            endScreen.transform.GetChild(i).gameObject.SetActive(false);
        }
        */

        clock.SetActive(true);
    }

    public void EndGame()
    {
        clock.SetActive(false);

        endScreen.SetActive(true);

        StartCoroutine("EndingAnimations");
    }

    IEnumerator EndingAnimations()
    {
        // fade in win/lose graphic
        // fade in win/lose text
        // fade in "Try Again"

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(endTextDelay);

            endScreen.transform.GetChild(i).gameObject.SetActive(true);
        }

        gameActive = false;

        yield break;
    }
}
