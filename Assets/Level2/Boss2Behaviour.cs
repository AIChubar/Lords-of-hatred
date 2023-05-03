using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss2Behaviour : MonoBehaviour
{
    public Transform[] wayPoints;
    
    public Transform[] leftWayPoints;
    
    public Transform startingWayPoint;
    
    public Transform startingWayPointLeft;
    
    public float moveSpeed;

    private int nextWayPoint;

    private int attackCount = 0;

    private int nextAttack = 10;

    private bool attacking = false;

    private AnimationClip attackAnimationClip;
    
    private AnimationClip disappearClip;

    Animator animator;

    private bool transitioning = false;
    
    private float levelCoef = 1.0f;
    
    private SpriteRenderer sprite;

    public SpriteRenderer transitionDiskSprite;
    
    private bool dying = false;
    
    private bool transitioned = false;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
        levelCoef += GameManager.gameManager.Character.levelsProgression[1] / 12f;
        if (levelCoef < 3)
        {
            moveSpeed *= levelCoef;
        }
        else
        {
            moveSpeed *= 3;
        }
        sprite = GetComponent<SpriteRenderer>();
        GetComponent<KillableEnemy>().healthSystem.OnHealthChanged += HealthSystem_OnHealthChangedAnimation;
        GameEvents.current.OnEnemyKilled += GameEvents_OnEnemyKilled;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].name == "Boss2Atk")
            {
                attackAnimationClip = clips[i];
            }
            else if (clips[i].name == "Boss2Disappear")
            {
                disappearClip = clips[i];
            }
        }

        transform.position = startingWayPoint.transform.position;
        nextWayPoint = 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!attacking && !transitioning && !dying)
            Move();
    }
    
    private void HealthSystem_OnHealthChangedAnimation(object sender, System.EventArgs e)
    {
        HealthSystem hs = (HealthSystem)sender;
        if (hs.Health < hs.HealthMax / 2 && hs.Health > 0 && !transitioned)
        {
            StartCoroutine(Transition(hs));
        }
        StartCoroutine(DamageReceivedAnimation(0.4f));
    }

    private void GameEvents_OnEnemyKilled(KillableEnemy enemy)
    {
        StartCoroutine(DeathAnimation());
    }
    
    private void Move()
    {
        if (transform.position == wayPoints[nextWayPoint].transform.position )
        {
            attackCount++;
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("Forward", false);

            if (attackCount == nextAttack)
            {
                Attack();
                return;
            }

            int newWayPoint = Random.Range(0, 8);
            while (newWayPoint == nextWayPoint)
            {
                newWayPoint = Random.Range(0, 8);
            }
            nextWayPoint = newWayPoint;

            float angle = Mathf.Atan2(wayPoints[nextWayPoint].transform.position.y - transform.position.y ,
                wayPoints[nextWayPoint].transform.position.x-transform.position.x) * 180 / Mathf.PI;

            if (angle <= 45 && angle >= -45)
            {
                animator.SetBool("Right", true);
            }
            else if (angle <= -45 && angle >= -135)
            {
                animator.SetBool("Forward", true);
            }
            else if (angle <= 135 && angle >= 45)
            {
                animator.SetBool("Back", true);
            }
            else if (angle <= -135 || angle >= 135)
            {
                animator.SetBool("Left", true);
            }
        }
        
        transform.position = Vector2.MoveTowards(transform.position,
                wayPoints[nextWayPoint].transform.position,
                moveSpeed * Time.deltaTime);
        
    }

    private void Attack()
    {
        nextAttack = Random.Range(4, 8);
        attackCount = 0;
        StartCoroutine(AttackAnimation(attackAnimationClip.length , 4));
    }
    
    private IEnumerator AttackAnimation(float duration, int attacks)
    {
        attacking = true;
        for (int i = 0; i < attacks; i++)
        {
            animator.Play("Boss2Atk");
            yield return new WaitForSeconds(duration);
            if (transitioning)
                break;
            transform.GetChild(0).GetComponent<Boss2Shooting>().Shoot();
        }
        attacking = false;

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

    private IEnumerator Transition(HealthSystem hs)
    {
        transitioning = true;
        animator.enabled = false;
        animator.enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        
        animator.Play("Boss2Disappear");
        for (float t = 0; t < 1; t += Time.deltaTime / disappearClip.length)
        {
            transitionDiskSprite.color = new Color(1, 1, 1, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        
        sprite.color  = new Color(1, 1, 1, 0);

        while (transform.position != startingWayPointLeft.transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                startingWayPointLeft.transform.position,
                10 * Time.deltaTime);
            yield return null;
        }
        
        transitionDiskSprite.color = new Color(1, 1, 1, 0);
        animator.Play("Boss2Idle");
        sprite.color  = new Color(1, 1, 1, 1);
        
        
        
        hs.Heal(hs.HealthMax);
        gameObject.GetComponent<Collider2D>().enabled = true;

        wayPoints = leftWayPoints;
        animator.enabled = false;
        animator.enabled = true;
        transitioning = false;
        transitioned = true;
        
        
    }

    private IEnumerator DeathAnimation()
    {
        dying = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        animator.Play("Boss2Disappear");
        
        for (float t = 0; t < 1; t += Time.deltaTime / disappearClip.length)
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        GameObject.FindGameObjectWithTag("LevelDoor").GetComponent<DoorScript>().OpenDoor();
        GetComponent<KillableEnemy>().healthSystem.OnHealthChanged -= HealthSystem_OnHealthChangedAnimation;
        GameEvents.current.OnEnemyKilled -= GameEvents_OnEnemyKilled;
        Destroy(this);
    }
}
