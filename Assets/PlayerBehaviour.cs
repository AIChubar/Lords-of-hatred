using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    private Vector2 movement;

    private bool colliding = false;

    private bool damageAnimation = false;
    
    private float animationTimer = 0.0f;

    private float animationDuration = 1.5f;

    public float health = 100f;
    
    SpriteRenderer sprite;

    private TextMeshProUGUI HP;
   
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        HP =  GameObject.Find("Interface/HP").GetComponent<TextMeshProUGUI>();
    }
    
    // Update is called once per frame
    void Update()
    {
        HP.text = "HP: " + health;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (colliding && !damageAnimation)
        {
            damageAnimation = true;
            animationTimer = 0.0f;
            health -= 35f;
        }

        if (Time.timeScale < 0.1f)
        {
            sprite.enabled = true;
        }
        else if (damageAnimation)
        {
            animationTimer += Time.deltaTime;
            if (sprite.enabled)
                sprite.enabled = false;
            else
                sprite.enabled = true;
            if (animationTimer > animationDuration)
            {
                damageAnimation = false;
                sprite.enabled = true;
            }
        }
        
    }

    public void TakeDamage(float damage)
    {
        
    }
    

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        colliding = true;

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        colliding = false;
    }
}
