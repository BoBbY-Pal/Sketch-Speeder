using Frolicode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Transform player; // Reference to the player's transform
    public TextMeshProUGUI scoreText; // Reference to the UI text component to display the score
    public Text gameOverScoreText; // Reference to the UI text component to display the score on game over screen.
    private float totalScore; // Score in meters
    

    void Start()
    {
       
    }

    public void UpdateScore(float currentScore)
    {
        if (!scoreText.gameObject.activeSelf)
        {
            scoreText.gameObject.SetActive(true);
        }

        totalScore = currentScore;
        // Update the score text, converting it to an integer for simplicity
        scoreText.text = "Score: " + ((int)currentScore).ToString() + "m";
    }
    
    public void UpdateScoreOnGameOverScreen()
    {
        if (!gameOverScoreText.gameObject.activeSelf)
        {
            gameOverScoreText.gameObject.SetActive(true);
        }
        // Update the score text, converting it to an integer for simplicity
        gameOverScoreText.text = ((int)totalScore).ToString();
    }
}