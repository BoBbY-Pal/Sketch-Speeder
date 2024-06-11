
    using Frolicode;
    using Sketch_Speeder;
    using UnityEngine;
    using UnityEngine.UI;

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public EndlessTilemapGenerator _tilemapGenerator;
        [SerializeField] private LinesDrawer linesDrawer;
        [SerializeField] private PlayerCharacter player;
        [SerializeField] private PowerUp powerUpsBtn;
        [SerializeField] private Text powerUpsTxt;
        public void StartGame()
        {
            Time.timeScale = 1f;
            _tilemapGenerator.enabled = true;
            linesDrawer.gameObject.SetActive(true);
            ScoreManager.Instance.UpdateScore(0);
            player.AddLives(player.maxLives);
            UiManager.Instance.UpdateHearts(player.maxLives);
            
            powerUpsBtn.gameObject.SetActive(true);
            powerUpsTxt.text = SavingSystem.Instance.Load().powerUps.ToString();
            powerUpsBtn.GetComponent<Button>().onClick.AddListener(ActivatePowerUp);
        }

        public void GameOver()
        {
            ExitGame();
            UiManager.Instance.TogglePanel("Panel", true);
            UiManager.Instance.TogglePanel("GameOver", true);
            ScoreManager.Instance.UpdateScoreOnGameOverScreen();
        }
        
        public void ActivatePowerUp()
        {
            if (SavingSystem.Instance.Load().powerUps > 0)
            {
                SavingSystem.Instance.DeductPowerUp(1);
                powerUpsBtn.ExtendSlowDownEffect();
                powerUpsTxt.text = SavingSystem.Instance.Load().powerUps.ToString();
            }
        }

        public void ExitGame()
        {
            _tilemapGenerator.enabled = false;
            linesDrawer.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
            powerUpsBtn.gameObject.SetActive(false);
            powerUpsBtn.GetComponent<Button>().onClick.RemoveListener(ActivatePowerUp);
        }
    }