using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace Sketch_Speeder.PlayFab
{
    public class LeaderboardEntry : MonoBehaviour
    {
        [SerializeField] Text rank;
        [SerializeField] Text score;
        [SerializeField] Text displayName;
    
        public void Setup(PlayerLeaderboardEntry player)
        {
            score.text = player.StatValue.ToString() + "m";
            rank.text = (player.Position + 1).ToString();
            displayName.text = player.DisplayName.ToString();
        }
    }
}