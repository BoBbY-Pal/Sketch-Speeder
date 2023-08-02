using System.Diagnostics;
using Frolicode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [Header("Player Lives")]
    public Image[] hearts; // The heart images to represent lives
    
    [Space ( 30f )]
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject livesPanel;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void PlayBtnPressed()
    {
        mainMenuPanel.SetActive(false);
        parentPanel.SetActive(false);
        TogglePanel("Lives Panel", true);
        GameManager.Instance.StartGame();
    }
    
    // Update the heart images to match the current number of lives
    public void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives; // Enable the heart if this life is active, disable it otherwise
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
        
    }
    
    
}
