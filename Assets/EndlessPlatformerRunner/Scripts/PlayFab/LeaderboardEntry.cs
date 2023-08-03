using UnityEngine;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] Text rank;
    [SerializeField] Text score;
    [SerializeField] Text displayName;
    
    public void Setup(PlayerLeaderboardEntry player)
    {
        score.text = player.StatValue.ToString();
        rank.text = player.Position + 1.ToString();
        displayName.text = player.DisplayName.ToString();
    }
}