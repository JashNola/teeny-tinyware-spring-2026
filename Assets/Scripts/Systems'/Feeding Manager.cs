using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using static Pigeon;

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

    // FeedPigeon variables
    int lastInput;
    public GameObject pigeonRestingPoints;

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
                    lastInput = i; 
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

        int restingPointIndex = position;

        GameObject selectedFeedingPoint = pigeonRestingPoints.transform.GetChild(restingPointIndex).gameObject;

        if (selectedFeedingPoint.GetComponent<WirePoint>().isOccupied)
        {
            FeedPigeon(restingPointIndex, selectedFeedingPoint);
        }


    }

    //TODO: check edge cases; thrown, but pigeon no longer there?
    //only check if pigeon there at end of thrown anim

    public void FeedPigeon(int num, GameObject obj)
    {
        Debug.Log("Feeding pigeon");

        for (int i = 0; i < PigeonManager.instance.pigeonArray.Length; i++)
        {
        
            if (PigeonManager.instance.pigeonArray[i] == null) continue;

            GameObject pointToCheck = PigeonManager.instance.pigeonArray[i].pigeonWirePoints;

            GameObject pigeonObj = PigeonManager.instance.pigeonArray[i].pigeonObject;

            if (obj != null && obj == pointToCheck && pigeonObj != null)
            {
                Pigeon targetPigeon = pigeonObj.GetComponent<Pigeon>();

                if (targetPigeon != null && targetPigeon.pigeonState == Pigeon.PigeonStates.Waiting)
                {
                    targetPigeon.pigeonState = Pigeon.PigeonStates.Eating;
                    targetPigeon.isFed = true;
                    //targetPigeon.pigeonState = PigeonStates.Flying;
                    break;
                }
            }
        }
    }





}
