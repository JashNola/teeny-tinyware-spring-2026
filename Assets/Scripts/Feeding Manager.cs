using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class FeedingManager : MonoBehaviour
{
    [Header("Object Reference")]
    public GameObject foodProjectile;

    [Header("Animation")]
    public SplineContainer[] splines;
    private SplineAnimate pathAnim;
    private Animation shrinkAnim;

    [Header("Modifiable Values")]
    public float throwCooldown;
    private float cooldownRemaining;
    private bool canThrow;

    private void Start()
    {
        pathAnim = foodProjectile.GetComponent<SplineAnimate>();
        shrinkAnim = foodProjectile.GetComponent<Animation>();
        cooldownRemaining = throwCooldown;
        canThrow = true;
    }

    /**
     * Handles key inputs and cooldowns
     */
    private void Update()
    {
        if (canThrow)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown((KeyCode)(48 + i)))
                {
                    Debug.Log("Detected key input " + 1);
                    canThrow = false;
                    if (i == 0) StartCoroutine("ThrowFood", 9);
                    else StartCoroutine("ThrowFood", i - 1);
                }
            }
        }
        else
        {
            if (cooldownRemaining > 0)
            {
                cooldownRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Food cooldown complete!");
                cooldownRemaining = throwCooldown;
                canThrow = true;
            }
        }
    }

    /**
     * Coroutine for throwing food, waiting until animation completes, then feeding pigeon
     */
    IEnumerator ThrowFood(int position)
    {
        Debug.Log("Throwing food at position " + position);
        // Set object to follow path to position
        foodProjectile.GetComponent<SplineAnimate>().Container = splines[position];

        // Play path and shrink animations
        pathAnim.Play();
        shrinkAnim.Play();

        // Wait until animation is over
        yield return new WaitForSeconds(0.5f);

        // Restart animation
        pathAnim.Restart(false);
        Debug.Log("Food has reached destination!");

        // Check if pigeon occupies position
        if (true)
        {        
            //FeedPigeon(position);
        }
    }

    //TODO: check edge cases; thrown, but pigeon no longer there?
    //only check if pigeon there at end of thrown anim
}
