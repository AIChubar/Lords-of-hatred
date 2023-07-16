using System.Collections;
using UnityEngine;

/// <summary>
/// Script containing behaviour of the third level boss.
/// </summary>
public class Boss3Behaviour : Boss
{
    [Header("Boss 3 laser object")]
    [SerializeField]
    private Laser BossLaser;

    [Header("Length of the attacking/sleeping stages")]
    [SerializeField] private float BossStageTime;

    private float stageTimer = 0.0f;
    
    private bool attacking = false;
    
    private Animator animator;

    private Collider2D bossCollider;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        bossCollider = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
        StopAttacking();
    }

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

    

    protected override void BeforeDeath()
    {
        Destroy(BossLaser.gameObject);
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
