using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeesMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    public Rigidbody2D rb;

    private Vector2 movement;

    private Vector2 DefaultPos;
    
    private float levelCoef = 1.0f;

    void Start()
    {
        DefaultPos = transform.position;
        levelCoef += GameManager.gameManager.Character.levelsProgression[0] / 10f;
        if (levelCoef >= 3)
            levelCoef = 3;
        moveSpeed *= levelCoef;
    }
    // Update is called once per frame
    void Update()
    {
        movement.y = 1.0f;
    }
    
    private void FixedUpdate()
    {
        var position = rb.position;
        var coef = moveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = new Vector2(position.x + movement.x * coef,
            position.y + movement.y * coef);

        if (rb.position.y > 10.0f)
        {
            newPos.y = -8.0f;
            newPos.x = DefaultPos.x + Random.Range(-1.0f, 1.5f);
        }
        
        rb.MovePosition(newPos);
    }
}
