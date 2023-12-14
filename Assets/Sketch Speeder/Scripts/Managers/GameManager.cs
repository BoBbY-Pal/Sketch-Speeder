
    using Frolicode;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public EndlessTilemapGenerator _tilemapGenerator;
        [SerializeField] private LinesDrawer linesDrawer;
        [SerializeField] private PlayerCharacter player;
        public void StartGame()
        {
            Time.timeScale = 1f;
            _tilemapGenerator.enabled = true;
            linesDrawer.gameObject.SetActive(true);
            ScoreManager.Instance.UpdateScore(0);
            player.AddLives(player.maxLives);
            UiManager.Instance.TogglePanel("Lives Panel", true);
            UiManager.Instance.UpdateHearts(player.maxLives);
        }

        public void GameOver()
        {
            _tilemapGenerator.enabled = false;
            linesDrawer.gameObject.SetActive(false);
            UiManager.Instance.TogglePanel("Panel", true);
            UiManager.Instance.TogglePanel("GameOver", true);
            ScoreManager.Instance.UpdateScoreOnGameOverScreen();
        }
        
    }