using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private PlayerStats PlayerStats;
    
    [Header("Levels")] 
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

    private GameObject LastSelectedButton;
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
        LastSelectedButton = Level1Button;

    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.current.SetSelectedGameObject(LastSelectedButton);
    }
    
    public void OnLevel1Clicked()
    {
        LastSelectedButton = Level1Button;
    }
    
    public void OnLevel2Clicked()
    {
        LastSelectedButton = Level2Button;
    }
    
    public void OnLevel3Clicked()
    {
        LastSelectedButton = Level3Button;
    }

    public void LoadSelectedLevel()
    {
        if (LastSelectedButton == Level1Button)
        {
            SceneController.LoadScene(2);
        }
        else if (LastSelectedButton == Level2Button)
        {
            SceneController.LoadScene(3);
        }
        else if (LastSelectedButton == Level3Button)
        {
            SceneController.LoadScene(4);
        }
    }
    
    public void LoadMainMenu()
    {
        SceneController.LoadScene(0);
    }
}
