﻿
    using System;
    using Frolicode;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private EndlessTilemapGenerator _tilemapGenerator;
        [SerializeField] private LinesDrawer linesDrawer;
        
        private void Start()
        {

        }

        public void StartGame()
        {
            _tilemapGenerator.enabled = true;
            linesDrawer.gameObject.SetActive(true);
        }

        public void GameOver()
        {
            UiManager.Instance.TogglePanel("Panel", true);
            UiManager.Instance.TogglePanel("GameOver", true);
            ScoreManager.Instance.UpdateScoreOnGameOverScreen();
        }

        public void RestartGame()
        {
            
        }
    }