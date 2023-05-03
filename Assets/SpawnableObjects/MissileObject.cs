using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileObject : MonoBehaviour
{

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<KillableEnemy>(out KillableEnemy enemyCommponent))
        {
            enemyCommponent.enemyHealth.Damage(GameManager.gameManager.Character.Damage.Value);
        }
        Destroy(gameObject);
    }
}
