using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Script containing behaviour of the second level boss.
/// </summary>
public class Boss2Behaviour : Boss
{
    [Header("List of waypoints boss moving between before transition")]
    [SerializeField]
    private Transform[] RightWayPoints;
    
    [Header("List of waypoints boss moving between after transition")]
    [SerializeField]
    private Transform[] LeftWayPoints;
    
    [Header("Starting waypoint")]
    [SerializeField]
    private Transform StartingWayPoint;
    
    [Header("Starting waypoint after transition")]
    [SerializeField]
    private Transform StartingWayPointLeft;
    
    [Header("Boss movement speed")]
    [SerializeField]
    private float MoveSpeed;

    private int nextWayPoint;

    private int attackCount = 0;

    private int nextAttack = 10;

    private bool attacking = false;

    private AnimationClip attackAnimationClip;
    
    private AnimationClip disappearClip;

    private Animator animator;

    private bool transitioning = false;
    

    private Boss2Shooting shootingComponent;
    
    [Header("Sprite of the transition disk")]
    [SerializeField]
    private SpriteRenderer TransitionDiskSprite;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SoundWalking;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SoundShooting;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SoundTransition;
    
    private bool transitioned = false;
    protected override void Awake()
    {
        base.Awake();
        shootingComponent = transform.GetChild(0).GetComponent<Boss2Shooting>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        MoveSpeed *= levelCoef;
        GetComponent<KillableEnemy>().healthSystem.OnHealthChanged += HealthSystem_OnHealthChangedTransition;
        AudioManager.instance.SetPitch(SoundWalking, levelCoef);
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

        transform.position = StartingWayPoint.transform.position;
        nextWayPoint = 4;
        
        AudioManager.instance.Play(SoundWalking);
    }

    void FixedUpdate()
    {
        if (!attacking && !transitioning && !dying)
            Move();
    }
    
    private void HealthSystem_OnHealthChangedTransition(object sender, System.EventArgs e)
    {
        HealthSystem hs = (HealthSystem)sender;
        if (hs.Health <= 0)
        {
            animator.enabled = false;
        }
        if (hs.Health < hs.HealthMax / 2 && hs.Health > 0 && !transitioned)
        {
            StartCoroutine(Transition(hs));
        }
        
    }

    private void Move()
    {
        if (transform.position == RightWayPoints[nextWayPoint].transform.position )
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

            float angle = Mathf.Atan2(RightWayPoints[nextWayPoint].transform.position.y - transform.position.y ,
                RightWayPoints[nextWayPoint].transform.position.x-transform.position.x) * 180 / Mathf.PI;

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

        if (!AudioManager.instance.IsPlaying(SoundWalking))
        {
            AudioManager.instance.Play(SoundWalking);
        }
        transform.position = Vector2.MoveTowards(transform.position,
            RightWayPoints[nextWayPoint].transform.position,
                MoveSpeed * Time.deltaTime);
        
    }

    private void Attack()
    {
        nextAttack = Random.Range(4, 8);
        attackCount = 0;
        StartCoroutine(AttackAnimation(attackAnimationClip.length , 4));
    }
    
    protected override void BeforeDeath()
    {
        AudioManager.instance.Stop(SoundWalking);
        AudioManager.instance.Stop(SoundShooting);
    }
    
    private IEnumerator AttackAnimation(float duration, int attacks)
    {
        AudioManager.instance.Stop(SoundWalking);

        attacking = true;
        for (int i = 0; i < attacks; i++)
        {
            animator.Play("Boss2Atk");
            yield return new WaitForSeconds(duration);
            if (transitioning || dying)
                break;
            shootingComponent.Shoot();
            AudioManager.instance.Play(SoundShooting);
        }
        attacking = false;

    }
    

    private IEnumerator Transition(HealthSystem hs)
    {
        AudioManager.instance.Stop(SoundWalking);
        AudioManager.instance.Stop(SoundShooting);
        transitioning = true;
        animator.enabled = false;
        animator.enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        
        animator.Play("Boss2Disappear");
        for (float t = 0; t < 1; t += Time.deltaTime / disappearClip.length)
        {
            TransitionDiskSprite.color = new Color(1, 1, 1, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        
        sprite.color  = new Color(1, 1, 1, 0);

        AudioManager.instance.Play(SoundTransition);
        while (transform.position != StartingWayPointLeft.transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                StartingWayPointLeft.transform.position,
                10 * Time.deltaTime);
            yield return null;
        }
        AudioManager.instance.Stop(SoundTransition);
        TransitionDiskSprite.color = new Color(1, 1, 1, 0);
        animator.Play("Boss2Idle");
        sprite.color  = new Color(1, 1, 1, 1);
        
        
        
        hs.Heal(hs.HealthMax);
        gameObject.GetComponent<Collider2D>().enabled = true;

        RightWayPoints = LeftWayPoints;
        animator.enabled = false;
        animator.enabled = true;
        transitioning = false;
        transitioned = true;
        
        AudioManager.instance.Play(SoundWalking);
    }
    
}
