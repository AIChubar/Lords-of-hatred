using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class KillableEnemy : MonoBehaviour
{
    [SerializeField]
    private float MaxHealth;

    [HideInInspector]
    public HealthSystem healthSystem;
    
    private float levelCoef = 1.0f;

    [SerializeField]
    private bool DeafultDeath = false;
    [SerializeField]
    private bool OneShotEnemy = false;

    private SpriteRenderer sprite;

    // Start is called before the first frame update

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
        levelCoef += GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] / 5f;
        if (levelCoef >= 4)
        {
            float overFixedCoef = levelCoef - 4f;
            levelCoef = 4f + overFixedCoef * 3;
        }
        MaxHealth *= levelCoef;
        healthSystem = new HealthSystem(MaxHealth);
        if (DeafultDeath)
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        if (healthSystem.Health == 0 || OneShotEnemy)
        {            
            StartCoroutine(DeathAnimation(0.03f));
        }
    }
    
    private IEnumerator DeathAnimation(float duration)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        
        for (float t = 0; t < 1; t += Time.deltaTime/duration)
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        Destroy(gameObject);
    }
    
}
