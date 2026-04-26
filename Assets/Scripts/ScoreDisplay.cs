using UnityEngine.UI; 
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreDisplay; 
    private void Update()
    {
        scoreDisplay.text = $"{GameplayController.instance.totalPigeonsFed} Pigeons Fed";
    }
}
