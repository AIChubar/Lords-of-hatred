using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    public Slider slider;

    public GameObject Enemy;

    private HealthSystem healthSystem;

    private float defaultHealth;

    void Start()
    {
        healthSystem = Enemy.GetComponent<KillableEnemy>().healthSystem;
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
