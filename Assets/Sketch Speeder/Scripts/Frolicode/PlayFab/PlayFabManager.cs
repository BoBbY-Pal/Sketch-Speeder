using System;
using System.Collections.Generic;
using DefaultNamespace;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using Sketch_Speeder.Advertising;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private string _titleID;

    // public static PlayFabManager Instance;
    public InputField loginUsernameField;
    public InputField loginPasswordField;
    
    public InputField signUpUsernameField;
    public InputField signUpPasswordField;
    public InputField signUpConfirmPasswordField;
    public Text loginTitle;
    public Text signUpTitle;
    
    public GameObject mainPanel;
    public GameObject loginPanel;
    public GameObject signUpPanel;
    
    public GameObject loginRegisterCanvas;
    public bool IsLoggedIn => PlayFabClientAPI.IsClientLoggedIn();

    private string playerFabID; // PlayFab ID of a local player.
    public string playerDisplayName;
    
    private List<PlayerLeaderboardEntry> players;
    public static Action<List<PlayerLeaderboardEntry>> OnPlayerListUpdated = delegate { };
    private void Awake()
    {
     
        Debug.Log("Is Main Panel Active on Awake: " + mainPanel.gameObject.activeInHierarchy);

        // Check login status at start of application
        if (IsLoggedIn)
        {
            // Deactivate login/register canvas and activate main canvas
            loginRegisterCanvas.SetActive(false);
            UiManager.Instance.TogglePanel("Panel", true);
            UiManager.Instance.TogglePanel("MainMenuPanel", true);
        }
        else
        {
            // Activate login/register canvas and deactivate main canvas
            loginRegisterCanvas.SetActive(true);
            mainPanel.SetActive(true);
            signUpPanel.SetActive(false);
            loginPanel.SetActive(false);
        }
        players = new List<PlayerLeaderboardEntry>();
    }

  

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = _titleID;
        }
    }

    public void RegisterUser()
    {
        string username = signUpUsernameField.text;
        string password = signUpPasswordField.text;
        string confirmPassword = signUpConfirmPasswordField.text;

        // basic validation
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            signUpTitle.text = "Username, password or confirm password is missing";
            Debug.LogError("Username, password or confirm password is missing");
            return;
        }
        if (password != confirmPassword)
        {
            signUpTitle.text = "Password and confirm password do not match";
            Debug.LogError("Password and confirm password do not match");
            return;
        }

        var request = new RegisterPlayFabUserRequest { Username = username, Password = password, RequireBothUsernameAndEmail = false };

        PlayFabClientAPI.RegisterPlayFabUser(request,
            result =>
            {
                signUpTitle.text = "User registered successfully";
                Debug.Log("User registered successfully");
                signUpPanel.SetActive(false);
                mainPanel.SetActive(true);
                SetPlayerDisplayName(username);
                LeaderboardManager.Instance.SetLeaderboardScore(0);
            }, 
            errorCallback =>
            {
                OnFailed(errorCallback);
                signUpTitle.text = errorCallback.ErrorMessage;
            });
    }
    public void Login()
    {
        
        string username = loginUsernameField.text;
        string password = loginPasswordField.text;

        // basic validation
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            loginTitle.text = "Username or password is missing";
            Debug.LogError("Username or password is missing");
            
            return;
        }

        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithPlayFab(request, 
            result =>
        {
            loginTitle.text = "User logged in successfully";
            Debug.Log("User logged in successfully");
           
            SetPlayerDisplayName(username);
            playerFabID = result.PlayFabId;
            LeaderboardManager.Instance.thisPlayerID = result.PlayFabId;
            loginPanel.SetActive(false);
            loginRegisterCanvas.SetActive(false);
            UiManager.Instance.TogglePanel("Panel", true);
            UiManager.Instance.TogglePanel("MainMenuPanel", true);
            AdManager.Instance.ShowInterstitialAd();
        },
            errorCallback =>
        {
            OnFailed(errorCallback);
            loginTitle.text = errorCallback.ErrorMessage;
            // Reactivate login/register canvas if login fails
            loginRegisterCanvas.gameObject.SetActive(true);
        });
    }

    public void LoginWithCustomID(string customId, bool createAccount)
    {
        var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnFailed);
    }
    
    private void OnFailed(PlayFabError error)
    {
        Debug.Log("There's a Error, process failed");
        UiManager.Instance.ActivatePopUp("Authorization Failed", error.ErrorMessage, 0);

        Debug.Log(error.GenerateErrorReport());
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("User logged in successfully");
    }

    public void SetPlayerDisplayName(string displayName)
    {
        playerDisplayName = displayName;
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            },
            result =>
            {
                Debug.Log($"Set display name was succeeded: {result.DisplayName}");
                PlayerPrefs.SetString("USERNAME", playerDisplayName);

            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
            }
        );
    }
    
    public void SetOnlineStatus(int status)
    {
        
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogError("User must be logged in to update score");
            return;
        }
            
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "IsPlayerOnline",
                    Value = status  // Status = 0 means offline, 1 means online.
                }
            }
        };
            
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnStatusUpdated, error =>
        {
            Debug.Log("Online status updating failed");
        });
    }

    private void OnStatusUpdated(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Online status successfully updated");
    }
    
    private void HandleGetPlayers()
    {
        GetOnlinePlayers();
    }
    
    public void GetOnlinePlayers()
    {
        // Define the parameters for the GetFriendLeaderboard request
        var request = new GetLeaderboardRequest
        {
            StatisticName = "IsPlayerOnline",
            MaxResultsCount = 10, // The maximum number of results you want to retrieve
        };

        // Call the GetFriendLeaderboard API
        PlayFabClientAPI.GetLeaderboard(request, OnGetOnlinePlayers, OnGetOnlinePlayersFailed);
    }


    // Callback function for successful API call
    private void OnGetOnlinePlayers(GetLeaderboardResult result)
    {
        Debug.Log($"Playfab get players list success: {result.Leaderboard.Count}");
        
        // Iterate over the results to get online player information
        foreach (var player in result.Leaderboard)
        {
            // Access player information (e.g., player.PlayFabId, player.DisplayName)
            if (player.StatValue == 1)
            {
                Debug.Log($"{player.DisplayName} is Online");
                players.Add(player);
                // var entry = Instantiate(playerDisplayPrefab, layoutTransform);
                // entry.Setup(player);
            }
            else
            {
                Debug.Log($"{player.DisplayName} is Offline");
            }
        }

        OnPlayerListUpdated?.Invoke(result.Leaderboard);
    }

    // Callback function for API call failure
    private static void OnGetOnlinePlayersFailed(PlayFabError error)
    {
        Debug.Log("Failed to get online users.");
    }
}
