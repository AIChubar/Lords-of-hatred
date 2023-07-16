using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for bosses.
/// </summary>
[RequireComponent(typeof(KillableEnemy))]
    public class Boss : MonoBehaviour
    {
        
        protected bool dying;
        protected SpriteRenderer sprite;
        protected float levelCoef = 1.0f;


        protected virtual void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            
        }

        protected virtual void Start()
        {
            GetComponent<KillableEnemy>().healthSystem.OnHealthChanged += HealthSystem_OnHealthChangedAnimation;
            GameEvents.current.OnEnemyKilled += GameEvents_OnEnemyKilled;
            levelCoef += GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] / 12f;
            if (levelCoef >= 2.5f)
            {
                levelCoef = 2.5f;
            }

        }

        private IEnumerator DeathAnimation()
        {
            GetComponent<Collider2D>().enabled = false;
            dying = true;
            for (float t = 0; t < 1; t += Time.deltaTime / 2)
            {
                sprite.color = new Color(Mathf.SmoothStep(0, 1, t), 0, 0, 1);
                yield return null;
            }

            float initialScale = transform.localScale.x;
            for (float t = 0; t < 1; t += Time.deltaTime / 1)
            {
                transform.localScale = new Vector3(initialScale, Mathf.SmoothStep(initialScale, 0, t), initialScale);
                yield return null;
            }

            GameObject.FindGameObjectWithTag("LevelDoor").GetComponent<DoorScript>().OpenDoor();
            GetComponent<KillableEnemy>().healthSystem.OnHealthChanged -= HealthSystem_OnHealthChangedAnimation;
            GameEvents.current.OnEnemyKilled -= GameEvents_OnEnemyKilled;
            Destroy(gameObject);
        }
        
        protected void HealthSystem_OnHealthChangedAnimation(object sender, EventArgs e)
        {
            StartCoroutine(DamageReceivedAnimation(0.4f));
        }

        private void GameEvents_OnEnemyKilled(KillableEnemy enemy)
        {
            if (gameObject != null && enemy.gameObject == gameObject)
            {
                BeforeDeath();
                StartCoroutine(DeathAnimation());
            }
        }

        protected virtual void BeforeDeath()
        {
            
        }
        
        private IEnumerator DamageReceivedAnimation(float duration)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / duration * 8)
            {
                sprite.color = new Color(Mathf.SmoothStep(1, 0, t), Mathf.SmoothStep(1, 0, t), Mathf.SmoothStep(1, 0, t));
                yield return null;
            }
            yield return new WaitForSeconds(duration * 3/4);
            for (float t = 0; t < 1; t += Time.deltaTime / duration * 8)
            {
                sprite.color = new Color(Mathf.SmoothStep(0, 1, t), Mathf.SmoothStep(0, 1, t), Mathf.SmoothStep(0, 1, t));
                yield return null;
            }

        }
    }