using UnityEngine;

public class PigeonDeleter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);

        if (other.CompareTag("PigeonLeaving"))
        {
            Destroy(other.gameObject);
        }
    }
}
