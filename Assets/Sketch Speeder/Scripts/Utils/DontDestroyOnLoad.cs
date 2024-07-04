using UnityEngine;

namespace Sketch_Speeder.Utils
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
