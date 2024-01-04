using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sketch_Speeder.Scripts.UI
{
    public class MissionUI : MonoBehaviour
    {
        public Text descriptionTxt;
        [FormerlySerializedAs("goal")] public Text goalTxt;
        [FormerlySerializedAs("progress")] public Text progressTxt;
        public Slider progressSlider;
    }
}