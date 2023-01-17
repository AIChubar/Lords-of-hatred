using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonObject : MonoBehaviour
{
    public float LifeTime = 18.0f;

    float timer = 0.0f;

    private CircleCollider2D col;

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);

        if (seconds >= 1)
        {
            col.enabled = true;
        }

        if (seconds >= LifeTime)
        {
            Destroy(gameObject);
        }
        
    }
    
}
