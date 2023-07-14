using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Character healthbar presented on each playable level inside HUD
/// </summary>
public class CharHealthBar : MonoBehaviour
{
    [Tooltip("Health bar slider")]
    [SerializeField]
    private Slider slider;

    private HealthSystem healthSystem;

    private float defaultHealth;
    
    void Start()
    {
        healthSystem = GameManager.gameManager.Character.healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        defaultHealth = GameManager.gameManager.Character.MaxHealth.BaseValue;
        RectTransform rt = GetComponent<RectTransform>();
        float width = 150f * healthSystem.HealthMax / defaultHealth;
        if (width > 800)
        {
            width = 800;
        }
        transform.position += new Vector3((width - rt.rect.width)/2f, 0, 0);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        slider.maxValue = healthSystem.HealthMax;
        slider.value = healthSystem.HealthMax;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        SetHealth(healthSystem.Health);
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
