using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    public Rigidbody2D rb;

    private Vector2 movement;

    private bool movingUp = true;
    
    private bool movingLeft = true;
    
    float timer = 0.0f;
    
    private int health = 200;

    private SpriteRenderer sprite;
    
    private TextMeshProUGUI HP;

    private int BossDamaged = 6;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        HP =  GameObject.Find("Canvas/HPBoss").GetComponent<TextMeshProUGUI>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!sprite.enabled)
            sprite.enabled = true;

        if (BossDamaged < 5)
        {
            sprite.enabled = false;
            BossDamaged++;
        }
        
        HP.text = "Boss HP: " + health;

        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);
        
        if (rb.position.y > 3.5f)
            movingUp = false;
        if (rb.position.y < -3.0f)
            movingUp = true;

        if (rb.position.x < -6.0f)
            movingLeft = false;
        if (rb.position.x > 5.5f)
        {
            movingLeft = true;
            timer = 0.0f;
            movement.x = 0.0f;
        }
        
        if (movingUp)
            movement.y = 0.6f;
        else
            movement.y = -0.6f;

        if (timer >= 8.0f)
            movement.y = 0.0f;
        if (timer >= 9.0f)
            if (movingLeft)
                movement.x = -8.0f;
            else
                movement.x = 4.0f;
    }
    
    private void FixedUpdate()
    {
        var position = rb.position;
        var coef = moveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = new Vector2(position.x + movement.x * coef,
            position.y + movement.y * coef);
        
        if (rb.position.x > 5.5f)
            newPos.x = 5.5f;
        
        rb.MovePosition(newPos);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Missile"))
        {
            health -= 10;
            BossDamaged = 0;
        }
        
    }
}
