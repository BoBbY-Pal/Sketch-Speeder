using System.Diagnostics;
using Frolicode;
using UnityEngine;
using UnityEngine.Serialization;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject rankingPanel;
    
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
        GameManager.Instance.StartGame();
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
        
    }
}
