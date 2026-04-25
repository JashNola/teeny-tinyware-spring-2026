using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using Pathfinding; 

public class PigeonAnimations : MonoBehaviour
{
    public AIPath aiPath;
    public GameObject pigeonGameObject; 
    Pigeon pigeonObjectScript; 
    Animator animator; 


    private void Start()
    {
        animator = this.GetComponent<Animator>();
        pigeonObjectScript = pigeonGameObject.GetComponent<Pigeon>(); 
    }


    private void Update()
    {
        
        // Checking which animation to play 

        if (pigeonObjectScript != null)
        {
            switch (pigeonObjectScript.pigeonState)
            {
                case Pigeon.PigeonStates.Flying:
                    animator.Play("Flying");
                    break;
                case Pigeon.PigeonStates.Waiting:
                    animator.Play("Idle");
                    break;
                case Pigeon.PigeonStates.Eating:
                    animator.Play("Fed");
                    break;
            }
        }

        else if (pigeonObjectScript == null)
        {
            Debug.Log("pigeonObjectScript is null");
        }



        // Checking if the sprite should be flipped

        if (aiPath.desiredVelocity.x >= 0.01f) // if we're moving to the right 
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);

        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
