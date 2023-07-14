using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main menu logic and controls.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField] 
    private Button continueGameButton;
    
    [SerializeField] 
    private Slider volumeSlider;

    [Header("Text on the button")] 
    [SerializeField] private TextMeshProUGUI continueText;
    
    private List<int> widths = new List<int>() { 960, 1280, 1792, 1920, 1920 };
    private List<int> heights = new List<int>() { 600, 720, 828, 960, 1080 };

    private bool isFullScreen = true;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("volume")) 
            PlayerPrefs.SetFloat("volume", 1.0f);
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }
    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            continueText.color = new Color(1, 1, 1, 0.4f);
        }
        SetResolution(3);
    }
    
    public void OnPlayGameClicked()
    {
        ButtonClickedSound();
        DataPersistenceManager.instance.NewGame();
        SceneController.LoadScene(2, 1, 1, 0.2f);
        DisableMenuButtons();
    }
    
    public void OnContinueGameClicked()
    {
        ButtonClickedSound();
        DataPersistenceManager.instance.LoadGame();
        SceneController.LoadScene(1, 1, 1, 0.2f);
        DisableMenuButtons();
    }

    public void OnQuitGameClicked()
    {
        ButtonClickedSound();
        Application.Quit();
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void SetScreenSize(Int32 index)
    {
        ButtonClickedSound();
        SetResolution(index);
    }

    public void SetResolution(Int32 index)
    {
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
    
    public void SetScreenMode(bool isFullScreen)
    {
        ButtonClickedSound();
        this.isFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
    
    public void SetVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void ButtonClickedSound()
    {
        AudioManager.instance.Play(ButtonClick);
    }
    
}
