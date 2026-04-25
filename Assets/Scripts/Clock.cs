using UnityEditor.ShaderGraph;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [Header("Object References")]
    public Transform clockHand;
    public StartEndController startEndController;
    public GameObject noonBG;
    public GameObject eveningBG;

    private float timeRemaining;
    private float degreesPerSecond = 6f; // 360 degrees over 60 seconds
    private string timeOfDay;
    private Color transparent = new Color(1, 1, 1, 0);

    private void OnEnable()
    {
        // Reset times
        timeRemaining = 60;
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
        clockHand.Rotate(0, 0, -degreesPerSecond * Time.deltaTime);

        // Change background based on time of day
        if (timeRemaining < 60 && timeOfDay == "morning")
        {
            Debug.Log("It is now approaching noon.");
            noonBG.GetComponent<Animation>().Play();
            timeOfDay = "noon";
        }
        if (timeRemaining < 30 && timeOfDay == "noon")
        {
            Debug.Log("It is now approaching evening.");
            eveningBG.GetComponent<Animation>().Play();
            timeOfDay = "evening";
        }
    }
}
