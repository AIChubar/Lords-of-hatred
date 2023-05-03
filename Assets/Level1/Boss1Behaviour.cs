using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss1Behaviour : MonoBehaviour
{

    public float moveSpeed;

    public Rigidbody2D rb;

    private Vector2 movement;

    float timer = 0.0f;
    
    private SpriteRenderer sprite;
    
    private bool movingUp = true;
    
    private bool movingLeft = true;

    private bool dying = false;

    private int lastTimeBossDamaged = 21;

    private float nextCharge;

    private float levelCoef = 1.0f;
    
    Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[0] / 10f;
        if (levelCoef >= 3)
        {
            levelCoef = 3;
        }
        sprite = GetComponent<SpriteRenderer>();
        GetComponent<KillableEnemy>().healthSystem.OnHealthChanged += HealthSystem_OnHealthChangedAnimation;
        GameEvents.current.OnEnemyKilled += GameEvents_OnEnemyKilled;
        nextCharge = Random.Range(5f, 10f) / levelCoef;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (dying)
        {
            movement.x = 0;
            movement.y = 0;
            return;
        }
        if (lastTimeBossDamaged < 20)
        {
            sprite.color = new Color(0, 0, 0);
            lastTimeBossDamaged++;
        }
        else
        {
            sprite.color = new Color(1,1,1);
        }
        
        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);
        
        if (rb.position.y > 3.5f)
            movingUp = false;
        if (rb.position.y < -3.0f)
            movingUp = true;

        if (rb.position.x < -6.0f)
            movingLeft = false;
        if (rb.position.x > 5.5f)
        {
            movingLeft = true;
            timer = 0.0f;
            nextCharge = Random.Range(5f, 10f) / levelCoef;
            movement.x = 0.0f;
        }
        
        if (movingUp)
            movement.y = 0.6f * levelCoef;
        else
            movement.y = -0.6f * levelCoef;

        if (timer >= nextCharge - 1)
            movement.y = 0.0f;
        if (timer >= nextCharge)
            if (movingLeft)
                movement.x = -8.0f * levelCoef;
            else
                movement.x = 4.0f * levelCoef;
    }
    
    private void HealthSystem_OnHealthChangedAnimation(object sender, System.EventArgs e)
    {
        lastTimeBossDamaged = 0;
    }

    private void GameEvents_OnEnemyKilled(KillableEnemy enemy)
    {
        if (gameObject != null && enemy.gameObject == gameObject)
        {
            StartCoroutine(DeathAnimation());
        }
        
    }
    

    private void FixedUpdate()
    {
        var position = rb.position;
        var coef = moveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = new Vector2(position.x + movement.x * coef,
            position.y + movement.y * coef);
        
        if (rb.position.x > 5.5f)
            newPos.x = 5.5f;
        
        rb.MovePosition(newPos);
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
        Destroy(this);
    }
}
