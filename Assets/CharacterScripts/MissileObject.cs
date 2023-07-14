using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MissileObject : MonoBehaviour
{
    private Vector2 Velocity;
    public GameObject ExplodingPS;
    public GameObject TrailPS;

    private GameObject TrailObject;
    
    
    
    private Rigidbody2D rb;

    private void Start()
    {
        TrailObject = Instantiate(TrailPS, transform.position, Quaternion.identity);
        rb = GetComponent<Rigidbody2D>();
        TrailObject.transform.rotation = transform.rotation;
        TrailObject.transform.SetParent(gameObject.transform.parent.transform);
    }

    private void Update()
    {
        TrailObject.transform.position = transform.position;
        
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(TrailObject, 1f);
            Destroy(gameObject, 1f);
        }
        //rb.MovePosition(rb.position + Velocity * Time.deltaTime );
    }

    public void SetVelocityRot(Vector2 velocity, Quaternion rotation)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        transform.rotation = rotation;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<KillableEnemy>(out KillableEnemy enemyComponent))
        {
            
            if(!enemyComponent.healthSystem.Damage(GameManager.gameManager.Character.Damage.Value))
                GameEvents.current.EnemyKilled(enemyComponent);
            GameObject explosionObject = Instantiate(ExplodingPS, transform.position, Quaternion.identity);
            explosionObject.transform.SetParent(gameObject.transform.parent.transform);
            Destroy(TrailObject);
            Destroy(gameObject);
            Destroy(explosionObject, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DestructableTileMap"))
        {
            Tilemap destructableTileMap = collision.gameObject.GetComponent<Tilemap>();
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts) {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                destructableTileMap.SetTile(destructableTileMap.WorldToCell(hitPosition), null);
            }
            GameObject explosionObject = Instantiate(ExplodingPS, transform.position, Quaternion.identity);
            explosionObject.transform.SetParent(gameObject.transform.parent.transform);
            Destroy(TrailObject);
            Destroy(gameObject);
            Destroy(explosionObject, 1f);
        }
        else if (collision.gameObject.TryGetComponent<KillableEnemy>(out KillableEnemy enemyComponent))
        {
            if(!enemyComponent.healthSystem.Damage(GameManager.gameManager.Character.Damage.Value))
                GameEvents.current.EnemyKilled(enemyComponent);
            GameObject explosionObject = Instantiate(ExplodingPS, transform.position, Quaternion.identity);
            explosionObject.transform.SetParent(gameObject.transform.parent.transform);
            Destroy(TrailObject);
            Destroy(gameObject);
            Destroy(explosionObject, 1f);
            
        }
    }
}
