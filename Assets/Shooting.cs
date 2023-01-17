using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject MisslePrefab;

    public float MissleForce = 10f;

    public float ShootingDelay = 1.0f;

    private float ShootingTimer = 1.1f;

    // Update is called once per frame
    private void Update()
    {
        ShootingTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1"))
        {
            if (ShootingTimer >= ShootingDelay)
            {
                Shoot();
                ShootingTimer = 0.0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject missile = Instantiate(MisslePrefab, FirePoint.position, FirePoint.rotation);
        missile.tag = "Missile";
        Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
        rb.AddForce(FirePoint.right*MissleForce, ForceMode2D.Impulse);
        
    }
}
