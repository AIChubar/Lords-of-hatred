using UnityEngine;
using UnityEngine.SceneManagement;

public enum PauseMode
{
    GameOver = 100,
    EscPause = 200,
    UnPaused = 300,
}

/// <summary>
/// Pause menu logic and controls.
/// </summary>
public class Pause : MonoBehaviour
{
    [HideInInspector]
    public PauseMode pauseMode;
    
    [Header("Pause canvas object")]
    [SerializeField]
    private GameObject PauseMenu;

    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    void Start()
    {
        pauseMode = PauseMode.UnPaused;
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        if (pauseMode == PauseMode.UnPaused)
        {
            UnSetPause();
        }
        else
        {
            SetPause(pauseMode);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMode == PauseMode.UnPaused)
            {
                pauseMode = PauseMode.EscPause;
            }
            else if (pauseMode == PauseMode.EscPause)
            {
                pauseMode = PauseMode.UnPaused;
            }
        }
    }

    public void SetPause(PauseMode mode)
    {
        AudioManager.instance.PauseSounds(true);

        PauseMenu.SetActive(true);
        bool gameOver = mode == PauseMode.GameOver;

        gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(!gameOver);
        gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(gameOver);
        
        Time.timeScale = 0f;
    }

    public void UnSetPause()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.PauseSounds(false);

    }

    public void GameOver()
    {
        pauseMode = PauseMode.GameOver;
    }

    public void RestartLevel()
    {
        Resume();
        SceneController.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void UpgradesMenu()
    {
        Resume();
        SceneController.LoadScene(1);
    }
    
    public void Resume()
    {
        AudioManager.instance.Play(ButtonClick);
        pauseMode = PauseMode.UnPaused;
    }

    public void Menu()
    {
        Resume();
        SceneController.LoadScene(0);
    }
}
