using Sketch_Speeder.Advertising;
using Sketch_Speeder.Mission_System;
using Sketch_Speeder.Utils;
using Sketch_Speeder.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Sketch_Speeder.Managers
{
    public class UiManager : Singleton<UiManager>
    {
        [Header("Player Lives")] public Image[] hearts; // The heart images to represent lives

        [Space(30f)] [SerializeField] private GameObject parentPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject rankingPanel;
        [SerializeField] private GameObject livesPanel;
        [SerializeField] private GameObject missionsPanel;
        [SerializeField] private PopUp _popUp;
        [SerializeField] private GameObject _pauseScreen;

        public void PlayBtnPressed()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            // AudioManager.instance.PlaySoundEffect(SoundEffect.Hit);
            mainMenuPanel.SetActive(false);
            parentPanel.SetActive(false);
            gameplayPanel.SetActive(true);
            GameManager.Instance.StartGame();
        }

        public void RestartGameBtnPressed()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            // AudioManager.instance.PlaySoundEffect(SoundEffect.Hit);
            TogglePanel("GameOver", false);
            TogglePanel("Pause Screen", false);
            PlayBtnPressed();
            AdManager.Instance.ShowInterstitialAd();
        }

        // Update the heart images to match the current number of lives
        public void UpdateHearts(int currentLives)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].enabled = i < currentLives; // Enable the heart if this life is active, disable it otherwise
            }
        }

        public void ActivatePauseScreen()
        {

            _pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }

        public void DeActivatePauseScreen()
        {
            _pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }

        public void ReturnToMainMenu()
        {
            TogglePanel("Gameplay", false);
            TogglePanel("Pause Screen", false);
            TogglePanel("Panel", true);
            TogglePanel("MainMenuPanel", true);
            Time.timeScale = 1f;
            GameManager.Instance.ExitGame();
        }

        public void ShowMissionsList()
        {
            missionsPanel.SetActive(true);
            MissionManager.Instance.CreateMissionsList();
        }

        public void HideMissionsList()
        {
            MissionManager.Instance.DestroyMissionsList();
            missionsPanel.SetActive(false);
        }

        /// <summary>
        /// Activates the small rectangular pop up.  
        /// </summary>
        /// <param name="title"></param> Title of the popup to be displayed at top.
        /// <param name="description"></param> Description/Message for the popup to be displayed at middle.
        /// <param name="timer"></param> Timer after which popup will be disabled. Pass 0 if don't want it to be disabled automatically.
        /// <returns></returns>
        public void ActivatePopUp(string title, string description, float timer)
        {
            _popUp.titleTxt.text = title;
            _popUp.description.text = description;

            _popUp.gameObject.SetActive(true);
            if (timer > 0)
            {
                StartCoroutine(_popUp.DisablePopUp(timer));
            }
        }

        public void TogglePanel(string panelToActivate, bool status)
        {
            if (parentPanel.name.Equals(panelToActivate))
            {
                parentPanel.SetActive(status);
            }
            else if (mainMenuPanel.name.Equals(panelToActivate))
            {
                mainMenuPanel.SetActive(status);
            }
            else if (gameOverPanel.name.Equals(panelToActivate))
            {
                gameOverPanel.SetActive(status);
            }
            else if (rankingPanel.name.Equals(panelToActivate))
            {
                rankingPanel.SetActive(status);
            }
            else if (livesPanel.name.Equals(panelToActivate))
            {
                livesPanel.SetActive(status);
            }
            else if (gameplayPanel.name.Equals(panelToActivate))
            {
                gameplayPanel.SetActive(status);
            }
            else if (_pauseScreen.name.Equals(panelToActivate))
            {
                _pauseScreen.SetActive(status);
            }

        }


    }
}
