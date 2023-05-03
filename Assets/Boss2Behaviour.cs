using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss2Behaviour : MonoBehaviour
{
    public Transform[] wayPoints;
    
    public Transform startingWayPoint;
    
    public float moveSpeed;

    private int nextWayPoint;

    private int attackCount = 0;

    private int nextAttack = 10;

    private bool attacking = false;
    
    
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.position = startingWayPoint.transform.position;
        nextWayPoint = 4;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }
    
    private void Move()
    {
        if (attacking)
            return;
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
            nextWayPoint = Random.Range(0, 8);

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
        nextAttack = Random.Range(4, 10);
        attackCount = 0;
        StartCoroutine(AttackAnimation(3.5f));
    }
    
    private IEnumerator AttackAnimation(float duration)
    {
        attacking = true;
        animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(duration);
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.6f);
        attacking = false;

    }
}
