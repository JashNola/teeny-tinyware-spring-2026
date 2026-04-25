using UnityEngine;

public class Clock : MonoBehaviour
{
    [Header("Object References")]
    public RectTransform clockHand;
    public StartEndController startEndController;

    private float timeRemaining;
    private float degreesPerSecond = 60.0f; // 360 degrees over 60 seconds

    private void OnEnable()
    {
        timeRemaining = 5;
    }

    void Update()
    {
        Debug.Log(timeRemaining + "seconds remaining");
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

        // Rotate clockhand
        //clockHand.localRotation = Quaternion.Euler(0, degreesPerSecond * Time.deltaTime, 0);
    }
}
