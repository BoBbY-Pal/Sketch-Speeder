﻿using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Sketch_Speeder.PlayFab
{
    public class PlayfabLogin : MonoBehaviour
    {
        [SerializeField] private string username;
        #region Unity Methods
        void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = "10E15";
            }
        }
        #endregion
        #region Private Methods
        private bool IsValidUsername()
        {
            bool isValid = username.Length is >= 3 and <= 24;

            return isValid;
        }
        private void LoginWithCustomId()
        {
            Debug.Log($"Login to Playfab as {username}");
            var request = new LoginWithCustomIDRequest { CustomId = username, CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginCustomIdSuccess, OnFailure);
        }
        private void UpdateDisplayName(string displayname)
        {
            Debug.Log($"Updating Playfab account's Display name to: {displayname}");
            var request = new UpdateUserTitleDisplayNameRequest { DisplayName = displayname };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnFailure);
        }
        #endregion
        #region  Public Methods
        public void SetUsername(string name)
        {
            username = name;
            PlayerPrefs.SetString("USERNAME", username);
        }
        public void Login()
        {
            if (!IsValidUsername()) return;

            LoginWithCustomId();
        }
        #endregion
        #region Playfab Callbacks
        private void OnLoginCustomIdSuccess(LoginResult result)
        {
            Debug.Log($"You have logged into Playfab using custom id {username}");
            UpdateDisplayName(username);
        }
        private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log($"You have updated the displayname of the playfab account!");
        }
        private void OnFailure(PlayFabError error)
        {
            Debug.Log($"There was an issue with your request: {error.GenerateErrorReport()}");
        }
        #endregion

    }
}