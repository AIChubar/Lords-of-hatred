using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss2Shooting : MonoBehaviour
{
    public GameObject MisslePrefab;

    private Boss2Behaviour Boss;

    public float MissileSpeed = 10f;

    public int MissileNumber = 6;

    private float AngleBetweenMissiles;
    
    private float levelCoef = 1.0f;
    private void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[1] / 12f;
        MissileNumber = (int)(MissileNumber * levelCoef);
        Physics2D.IgnoreLayerCollision(MisslePrefab.layer, MisslePrefab.layer, true);
        Boss = transform.parent.GetComponent<Boss2Behaviour>();
        AngleBetweenMissiles = 360f / MissileNumber;
    }

    public void Shoot()
    {
        float startingAngleShift = Random.Range(0, AngleBetweenMissiles/2f);
        for (int i = 0; i < MissileNumber; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, -180f + startingAngleShift + i * AngleBetweenMissiles);
            GameObject missile = Instantiate(MisslePrefab, transform.position, rot);
            missile.GetComponent<Boss2Missile>().SetSpeed(MissileSpeed);
            if (levelCoef <= 3f)
                missile.transform.localScale *= levelCoef;
            else
                missile.transform.localScale *= 3f;

        }
    }
}
