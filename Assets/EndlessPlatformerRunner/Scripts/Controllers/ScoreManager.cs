using Frolicode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Transform player; // Reference to the player's transform
    public TextMeshProUGUI scoreText; // Reference to the UI text component to display the score

    

    void Start()
    {
       
    }

    public void UpdateScore(float score)
    {
        if (!scoreText.gameObject.activeSelf)
        {
            scoreText.gameObject.SetActive(true);
        }
        // Update the score text, converting it to an integer for simplicity
        scoreText.text = "Score: " + ((int)score).ToString() + "m";
    }
    
}