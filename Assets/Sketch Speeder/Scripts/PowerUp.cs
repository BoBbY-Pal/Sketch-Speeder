using System.Collections;
using UnityEngine;

namespace Sketch_Speeder.Scripts
{
    public class PowerUp : MonoBehaviour
    {
        private static float slowDownDuration = 0f;
        private static bool isCoroutineRunning = false;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Border"))
            {
                Debug.Log("Hit with Border");
                DeactivatePowerUp();
            }
            else if (collision.CompareTag("Player"))
            {
                Debug.Log("PowerUp consumed by player");
                // collision.GetComponent<PlayerCharacter>().ExtendSlowDownEffect();
                ExtendSlowDownEffect();
                DeactivatePowerUp();
            }
        }

        private void ExtendSlowDownEffect()
        {
            // StopCoroutine(nameof(SlowDownGame)); // Stop any existing slowdown coroutine
            slowDownDuration += 1f; // Add more time to the slowdown effect
            if (!isCoroutineRunning)
            {
                StartCoroutine(SlowDownGame());
            }
        }

        private IEnumerator SlowDownGame()
        {
            isCoroutineRunning = true;
            Time.timeScale = 0.6f;

            while (slowDownDuration > 0)
            {
                yield return new WaitForSecondsRealtime(1f);
                slowDownDuration -= 1f;
            }
            
            Time.timeScale = 1f;
            isCoroutineRunning = false;

        }

        private void DeactivatePowerUp()
        {
            gameObject.SetActive(false);
        }
    }
}