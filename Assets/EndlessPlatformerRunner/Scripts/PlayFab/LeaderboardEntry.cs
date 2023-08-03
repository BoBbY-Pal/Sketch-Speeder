using UnityEngine;
using TMPro;
using PlayFab.ClientModels;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] TMP_Text score;

    public void Setup(PlayerLeaderboardEntry player)
    {
        score?.SetText($"{player.Position+1}. {player.DisplayName}: {player.StatValue}");
    }
}