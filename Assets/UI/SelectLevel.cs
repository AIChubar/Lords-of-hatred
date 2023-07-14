using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Level selecting logic for the Upgrade Menu scene.
/// </summary>
public class SelectLevel : MonoBehaviour
{
    [SerializeField] private PlayerStats PlayerStats;
    
    [SerializeField]
    private GameObject Level1Button;

    [SerializeField]
    private TextMeshProUGUI Level1Progress;

    [SerializeField] 
    private GameObject Level2Button;
    
    [SerializeField]
    private TextMeshProUGUI Level2Progress;

    [SerializeField] 
    private GameObject Level3Button;
    
    [SerializeField]
    private TextMeshProUGUI Level3Progress;

    private GameObject lastSelectedButton;

    private bool buttonsDisabled;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    
    // Start is called before the first frame update
    void Start()
    {
        Level1Progress.text = PlayerStats.levelsProgression[0].ToString();
        Level2Progress.text = PlayerStats.levelsProgression[1].ToString();
        Level3Progress.text = PlayerStats.levelsProgression[2].ToString();
        
        if (PlayerStats.levelsProgression[0] < 1)
        { 
            Level2Button.GetComponent<Button>().interactable = false;
            Level2Button.GetComponent<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.4f);
        }

        if (PlayerStats.levelsProgression[1] < 1)
        {
            Level3Button.GetComponent<Button>().interactable = false;
            Level3Button.GetComponent<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.4f);
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Level1Button);
        lastSelectedButton = Level1Button;

    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.current.SetSelectedGameObject(lastSelectedButton);
    }
    
    public void OnLevel1Clicked()
    {
        if (buttonsDisabled)
            return;
        AudioManager.instance.Play(ButtonClick);
        lastSelectedButton = Level1Button;
    }
    
    public void OnLevel2Clicked()
    {
        if (buttonsDisabled)
            return;
        AudioManager.instance.Play(ButtonClick);
        lastSelectedButton = Level2Button;
    }
    
    public void OnLevel3Clicked()
    {
        if (buttonsDisabled)
            return;
        AudioManager.instance.Play(ButtonClick);
        lastSelectedButton = Level3Button;
    }

    public void LoadSelectedLevel()
    {
        if (buttonsDisabled)
            return;
        DisableMenuButtons();
        AudioManager.instance.Play(ButtonClick);
        if (lastSelectedButton == Level1Button)
        {
            SceneController.LoadScene(2);
        }
        else if (lastSelectedButton == Level2Button)
        {
            SceneController.LoadScene(3);
        }
        else if (lastSelectedButton == Level3Button)
        {
            SceneController.LoadScene(4);
        }
    }
    
    public void LoadMainMenu()
    {
        if (buttonsDisabled)
            return;
        AudioManager.instance.Play(ButtonClick);
        DisableMenuButtons();
        SceneController.LoadScene(0);
    }
    
    private void DisableMenuButtons()
    {
        buttonsDisabled = true;
    }
}
