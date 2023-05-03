using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFiringEnemy : MonoBehaviour
{
    public GameObject MisslePrefab;

    public Collider2D DetectionCollider;

    public GameObject InstantiatedObjectParent;

    public float MissileSpeed;

    public float ShootingDelay;

    private float ShootingTimer = 0.0f;

    private bool colliding = false;

    private PlayerAnimation player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootingTimer += Time.deltaTime;
        if (colliding && ShootingTimer > ShootingDelay)
        {
            Shoot(player.transform.position);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<PlayerAnimation>(out PlayerAnimation player))
        {
            colliding = true;
            this.player = player;
        }
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<PlayerAnimation>(out PlayerAnimation player))
        {
            colliding = false;
        }
    }

    private void Shoot(Vector3 heroPosition)
    {
        ShootingTimer = 0.0f;
        GameObject missile = Instantiate(MisslePrefab, transform.position, Quaternion.identity);
        missile.transform.SetParent(InstantiatedObjectParent.transform);
        missile.tag = "Missile";
        Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
        Vector3 direction = heroPosition - transform.position;
        Vector2 velocity = new Vector2(direction.x, direction.y).normalized * MissileSpeed;
        StatueMissile missileComp = missile.GetComponent<StatueMissile>();
        missileComp.SetVelocity(velocity);
    }
}
