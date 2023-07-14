using UnityEngine;

/// <summary>
/// Script for level two boss missile logic.
/// </summary>
public class Boss2Missile : MonoBehaviour
{
    private float MissileSpeed;

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

    public void SetSpeed(float speed)
    {
        MissileSpeed = speed;
        Vector2 velocity = new Vector2(  Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad) * MissileSpeed,
            Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad) * MissileSpeed);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }
}
