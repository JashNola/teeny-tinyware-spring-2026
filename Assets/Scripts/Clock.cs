using UnityEngine;

public class Clock : MonoBehaviour
{
    [Header("Object References")]
    public Transform clockHand;
    public StartEndController startEndController;
    public GameObject noonBG;
    public GameObject eveningBG;

    public float maxTime = 60f;
    private float timeRemaining;

    private string timeOfDay;
    private Color transparent = new Color(1, 1, 1, 0);

    private void OnEnable()
    {
        // Reset times
        timeRemaining = maxTime;
        timeOfDay = "morning";

        // Reset backgrounds
        noonBG.GetComponent<SpriteRenderer>().color = transparent;
        eveningBG.GetComponent<SpriteRenderer>().color = transparent;
    }

    void Update()
    {
        //Debug.Log(timeRemaining + "seconds remaining");

        // Decrement remaining time
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }

        // if time runs out, end game
        else
        {
            Debug.Log("Time has run out!");
            timeRemaining = 0;
            startEndController.EndGame();
        }

        // Rotate clock hand
        clockHand.Rotate(0, 0, -360f / maxTime * Time.deltaTime);

        // Change background based on time of day
        if (timeRemaining < maxTime && timeOfDay == "morning")
        {
            Debug.Log("It is now approaching noon.");
            noonBG.GetComponent<Animation>().Play();
            timeOfDay = "noon";
        }
        if (timeRemaining < maxTime/2 && timeOfDay == "noon")
        {
            Debug.Log("It is now approaching evening.");
            eveningBG.GetComponent<Animation>().Play();
            timeOfDay = "evening";
        }
    }
}
