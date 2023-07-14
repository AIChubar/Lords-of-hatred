using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Behaviour : MonoBehaviour
{
    private SpriteRenderer sprite;


    [SerializeField]
    private Laser BossLaser;

    [SerializeField] private float BossStageTime;

    private float stageTimer = 0.0f;

    private bool dying = false;

    private bool attacking = false;
    
    private Animator animator;

    private Collider2D bossCollider;
    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StopAttacking();
        GetComponent<KillableEnemy>().healthSystem.OnHealthChanged += HealthSystem_OnHealthChangedAnimation;
        GameEvents.current.OnEnemyKilled += GameEvents_OnEnemyKilled;
    }

    // Update is called once per frame
    void Update()
    {
        stageTimer += Time.deltaTime;
        if (stageTimer > BossStageTime)
        {
            if (attacking)
            {
                StopAttacking();
            }
            else
            {
                StartAttacking();
            }

            stageTimer = 0.0f;
        }
    }

    private void StartAttacking()
    {
        StartCoroutine(ChangeTint(0.4f, 1.0f));
        attacking = true;
        BossLaser.EnableLaser();
        bossCollider.enabled = true;
        animator.Play("Boss3Awake");
    }


    private void StopAttacking()
    {
        StartCoroutine(ChangeTint(1.0f, 0.4f));
        attacking = false;
        bossCollider.enabled = false;
        BossLaser.DisableLaser();
        animator.Play("Boss3Sleeping");
    }
    private void HealthSystem_OnHealthChangedAnimation(object sender, System.EventArgs e)
    {
        if (!dying)
        {    
            StartCoroutine(DamageReceivedAnimation(0.4f));
        }
    }

    private void GameEvents_OnEnemyKilled(KillableEnemy enemy)
    {
        
        if (enemy.gameObject == gameObject)
            StartCoroutine(DeathAnimation());
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

    private IEnumerator DeathAnimation()
    {
        Destroy(BossLaser.gameObject);
        GetComponent<Collider2D>().enabled = false;
        dying = true;
        for (float t = 0; t < 1; t += Time.deltaTime / 2)
        {
            sprite.color = new Color(Mathf.SmoothStep(0, 1, t), 0, 0, 1);
            yield return null;
        }

        float initialScale = transform.localScale.x;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            transform.localScale = new Vector3(initialScale, Mathf.SmoothStep(initialScale, 0, t), initialScale);
            yield return null;
        }

        GameObject.FindGameObjectWithTag("LevelDoor").GetComponent<DoorScript>().OpenDoor();
        GetComponent<KillableEnemy>().healthSystem.OnHealthChanged -= HealthSystem_OnHealthChangedAnimation;
        GameEvents.current.OnEnemyKilled -= GameEvents_OnEnemyKilled;
        Destroy(this);
    }

    private IEnumerator ChangeTint(float from, float to)
    {
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            sprite.color = new Color(Mathf.SmoothStep(from, to, t), Mathf.SmoothStep(from, to, t),
                Mathf.SmoothStep(from, to, t));
            yield return null;
        }
    }
}
