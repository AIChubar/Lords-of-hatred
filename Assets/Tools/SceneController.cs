using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for manipulating scenes, smooth scenes changing.
/// </summary>
public class SceneController : MonoBehaviour
{
    [Tooltip("Image that is going to be expanded to the whole screen during scene transition")]
    [SerializeField]
    private Image fader;
    
    private static SceneController instance;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound LevelMusic;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound MenuMusic;

    private int currentSceneIndex = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            fader.rectTransform.sizeDelta = new Vector2(Screen.width + 2000, Screen.height + 2000);
            fader.gameObject.SetActive(false);
        }
    }
    
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        AudioManager.instance.Play(MenuMusic);
    }

    public static void LoadScene(int index, float closingDuration = 1f, float openingDuration = 1f, float waitTime = 0f)
    {
        instance.StartCoroutine(instance.FadeScene(index, closingDuration, openingDuration, waitTime));
    }

    private IEnumerator FadeScene(int index, float closingDuration, float openingDuration, float waitTime)
    {
        Time.timeScale = 1.0f;
        fader.gameObject.SetActive(true);
        if (currentSceneIndex <= 1 && index > 1)
        {
            AudioManager.instance.Stop(MenuMusic);
        }
        AudioManager.instance.Stop(LevelMusic);
        for (float t = 0; t < 1; t += Time.deltaTime / closingDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(index);

        yield return new WaitForSeconds(waitTime);
        
        if (index <= 1 && currentSceneIndex > 1)
            AudioManager.instance.Play(MenuMusic);
        else if (index > 1)
            AudioManager.instance.Play(LevelMusic);

        for (float t = 0; t < 1; t += Time.deltaTime / openingDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        fader.gameObject.SetActive(false);

        currentSceneIndex = index;
    }
    
    
}
