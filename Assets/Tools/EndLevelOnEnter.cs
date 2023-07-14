using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Change the scene when player enters the trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EndLevelOnEnter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerAnimation playerAnimation))
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] += 1;
            GameManager.gameManager.Character.availableStatPoints += 6;
            SceneController.LoadScene(1, 0.5f);
        }
    }
}
