using UnityEngine;

/// <summary>
/// Script for level three statue missile logic.
/// </summary>
public class StatueMissile : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.TryGetComponent<MissileObject>(out MissileObject missile))
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
