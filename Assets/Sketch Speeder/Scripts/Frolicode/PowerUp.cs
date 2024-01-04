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
                ExtendSlowDownEffect();
                DeactivatePowerUp();
            }
        }

        private void ExtendSlowDownEffect()
        {
            slowDownDuration += 1f;
            Debug.Log($"Extending slow down effect, new duration: {slowDownDuration}");

            if (!isCoroutineRunning)
            {
                StartCoroutine(SlowDownGame());
            }
            else
            {
                StopCoroutine(nameof(SlowDownGame));
                StartCoroutine(SlowDownGame());
            }
        }

        private IEnumerator SlowDownGame()
        {
            isCoroutineRunning = true;
            Debug.Log("Starting slow down effect");
            Time.timeScale = 0.6f;

            while (slowDownDuration > 0)
            {
                yield return new WaitForSecondsRealtime(1f);
                slowDownDuration -= 1f;
                Debug.Log($"Slow down effect ongoing, remaining duration: {slowDownDuration}");
            }

            Time.timeScale = 1f;
            isCoroutineRunning = false;
            Debug.Log("Slow down effect ended, time scale reset to 1");
        }

        private void DeactivatePowerUp()
        {
            Debug.Log("Deactivating power up");
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}