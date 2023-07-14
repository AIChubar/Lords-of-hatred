using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Health Bar that can be attached to an enemy.
/// </summary>
public class HealthBarEnemy : MonoBehaviour
{
    private Slider slider;

    [Tooltip("Enemy which health is tracked by this health bar")]
    [SerializeField]
    private GameObject Enemy;

    private HealthSystem healthSystem;

    private float defaultHealth;

    void Start()
    {
        healthSystem = Enemy.GetComponent<KillableEnemy>().healthSystem;
        slider = GetComponent<Slider>();
        SetMaxHealth(healthSystem.HealthMax);
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        SetHealth(healthSystem.Health);
        if (healthSystem.Health <= 0)
            StartCoroutine(Disappear());
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
    }

    private IEnumerator Disappear()
    {
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        CanvasGroup cnv = GetComponentInParent<CanvasGroup>();
        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            cnv.alpha =  Mathf.Lerp(1, 0, t);
            yield return null;
        }
        Destroy(cnv.gameObject);
    }
    
}
