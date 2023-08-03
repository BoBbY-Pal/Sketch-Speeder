using System;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private string _titleID;
    private SpriteRenderer _spriteRenderer;

    private Camera _camera;

    // public static PlayFabManager Instance;
    public TMP_InputField loginUsernameField;
    public TMP_InputField loginPasswordField;
    
    public TMP_InputField signUpUsernameField;
    public TMP_InputField signUpPasswordField;
    public TMP_InputField signUpConfirmPasswordField;
    
    public CanvasGroup mainPanel;
    public CanvasGroup loginPanel;
    public CanvasGroup signUpPanel;
    
    public Canvas loginRegisterCanvas;
    public bool IsLoggedIn => PlayFabClientAPI.IsClientLoggedIn();
    public Button closeButtonLogin;
    public Button closeButtonSignup;
    
    public string playerFabID;
    public string playerDisplayName;
    [SerializeField] private GameObject challengeHandler;
    [SerializeField] private GameObject MainMenu;
    private List<PlayerLeaderboardEntry> players;
    public static Action<List<PlayerLeaderboardEntry>> OnPlayerListUpdated = delegate { };
    private void Awake()
    {
        // if (Instance == null)
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else if (Instance != this)
        // {
        //     Destroy(gameObject);
        // }
        
        // Make MainPanel visible, and hide the sign up, and game panels at the start
        ShowCanvasGroup(mainPanel);
        HideCanvasGroup(signUpPanel);

        // Only hide loginPanel if user is already logged in
        if (IsLoggedIn)
        {
            HideCanvasGroup(loginPanel);
        }

        Debug.Log("Is Main Panel Active on Awake: " + mainPanel.gameObject.activeInHierarchy);

        closeButtonLogin.onClick.AddListener(CloseLoginAndSignUpPanels);
        closeButtonSignup.onClick.AddListener(CloseLoginAndSignUpPanels);

        // Check login status at start of application
        if (IsLoggedIn)
        {
            // Deactivate login/register canvas and activate main canvas
            loginRegisterCanvas.gameObject.SetActive(false);
            SceneManager.LoadScene("Space Invaders");
        }
        else
        {
            //ShowCanvasGroup(loginPanel);
            // Activate login/register canvas and deactivate main canvas
            loginRegisterCanvas.gameObject.SetActive(true);
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
            Debug.LogError("Username, password or confirm password is missing");
            return;
        }
        if (password != confirmPassword)
        {
            Debug.LogError("Password and confirm password do not match");
            return;
        }

        var request = new RegisterPlayFabUserRequest { Username = username, Password = password, RequireBothUsernameAndEmail = false };

        PlayFabClientAPI.RegisterPlayFabUser(request,
            result =>
            {
                Debug.Log("User registered successfully");
                HideCanvasGroup(signUpPanel);
                ShowCanvasGroup(mainPanel); // Show the main UI of Canvas - Login/Register
                SetPlayerDisplayName(username);
            }, OnFailed);
    }
    public void Login()
    {
        
        string username = loginUsernameField.text;
        string password = loginPasswordField.text;

        // basic validation
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
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
            Debug.Log("User logged in successfully");
            SetOnlineStatus(1);
            SetPlayerDisplayName(username);
            playerFabID = result.PlayFabId;
            loginPanel.gameObject.SetActive(false);
            MainMenu.SetActive(true);
        },
            errorCallback =>
        {
            OnFailed(errorCallback);
            
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
    
   
    
    private void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        Debug.Log("HideCanvasGroup is called. Stack Trace: " + Environment.StackTrace);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false); // set game object to inactive
    }
    private void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Debug.Log("MainPanel: " + mainPanel);
        Debug.Log("LoginPanel: " + loginPanel);
        Debug.Log("SignUpPanel: " + signUpPanel);
    }
    
    public void ShowLoginPanel()
    {
        Debug.Log("ShowLoginPanel called");

        HideCanvasGroup(mainPanel);
        ShowCanvasGroup(loginPanel);
        HideCanvasGroup(signUpPanel);
    }

    public void ShowSignUpPanel()
    {
        Debug.Log("ShowSignUpPanel called");

        HideCanvasGroup(mainPanel);
        ShowCanvasGroup(signUpPanel);
        HideCanvasGroup(loginPanel);
    }
    
    public void CloseLoginAndSignUpPanels()
    {
        Debug.Log("CloseLoginAndSignUpPanels called");

        HideCanvasGroup(loginPanel);
        HideCanvasGroup(signUpPanel);
        ShowCanvasGroup(mainPanel);
    }
}
