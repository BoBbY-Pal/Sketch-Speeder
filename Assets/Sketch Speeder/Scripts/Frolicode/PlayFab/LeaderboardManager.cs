using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace Sketch_Speeder.PlayFab
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance;
        public string thisPlayerID; // PlayFab ID of a local player.
        
        [Header("UI")]
        [SerializeField] GameObject panel;
        [SerializeField] LayoutGroup layoutGroup;
        [SerializeField] LeaderboardEntry localPlayerEntry;
        [SerializeField] LeaderboardEntry entryPrefab;

        List<LeaderboardEntry> _entries = new List<LeaderboardEntry>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        public void SetLeaderboardScore(int score)
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
                        StatisticName = "PlayerHighestScore",
                        Value = score
                    }
                }
            };
            
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdated, error =>
            {
                Debug.Log("Leaderboard updating failed");
            });
        }

        private void OnLeaderboardUpdated(UpdatePlayerStatisticsResult result)
        {
            Debug.Log("Leaderboard successfully updated");
        }

        public void GetLeaderboard()
        {
            // yield return new WaitForSeconds(.5f);
            var request = new GetLeaderboardRequest
            {
                StatisticName = "PlayerHighestScore",
                StartPosition = 0,
                MaxResultsCount = 10
            };
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, error =>
            {
                Debug.Log("Couldn't get leaderboard");
            });
        }

        private void OnGetLeaderboard(GetLeaderboardResult result)
        { 
            // Clear previous entries
            foreach (LeaderboardEntry entry in _entries)
            {
                Destroy(entry.gameObject);
            }
            _entries.Clear();

            panel.SetActive(true);
            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
            {
                if (player.PlayFabId == thisPlayerID)
                {
                    localPlayerEntry.Setup(player);
                }
                else
                {
                    var entry = Instantiate(entryPrefab, layoutGroup.transform);
                    entry.Setup(player);
                    _entries.Add(entry);
                }
            }
        }
    }
}