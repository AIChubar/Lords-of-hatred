using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueMissile : MonoBehaviour
{
    private float MissileSpeed;

    private void Start()
    {
        Destroy(gameObject, 4f);

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<Boss2Behaviour>(out Boss2Behaviour Boss) ||
            col.gameObject.TryGetComponent<Boss2Missile>(out Boss2Missile Missile))
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        else if (!col.gameObject.TryGetComponent<MissileObject>(out MissileObject missile))
        {
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }
}
