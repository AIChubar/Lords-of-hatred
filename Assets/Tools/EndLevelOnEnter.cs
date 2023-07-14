using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerAnimation>(out PlayerAnimation playerAnimation))
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] += 1;
            GameManager.gameManager.Character.availableStatPoints += 6;
            SceneController.LoadScene(1, 0.5f, 1f);
        }
    }
}
