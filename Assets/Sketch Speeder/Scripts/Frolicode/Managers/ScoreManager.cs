﻿using Sketch_Speeder.PlayFab;
using Sketch_Speeder.Utils;
using TMPro;
using UnityEngine.UI;

namespace Sketch_Speeder.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
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
            if (scoreText.gameObject.activeSelf)
            {
                scoreText.gameObject.SetActive(false);
            }
            if (!gameOverScoreText.gameObject.activeSelf)
            {
                gameOverScoreText.gameObject.SetActive(true);
            }
            // Update the score text, converting it to an integer for simplicity
            gameOverScoreText.text = ((int)totalScore).ToString() + "m";
        
            LeaderboardManager.Instance.SetLeaderboardScore((int)totalScore);
        }
    }
}