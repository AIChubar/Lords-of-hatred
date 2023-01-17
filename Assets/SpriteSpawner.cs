using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpawner : MonoBehaviour
{
    public Transform TopLeft;
    public Transform DownRight;

    public Sprite SpriteToSpawn;

    private Transform _spawnedObjectsParent;

    private int onSecondAdded;
    
    float timer = 0.0f;

    private void Start()
    {
        _spawnedObjectsParent = transform.Find("SpawnedObjects");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);

        if (seconds > 0 && seconds % 4 == 0 && seconds != onSecondAdded)
        {
            onSecondAdded = seconds;
            SpawnPosion();
        }
        
    }
    

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnPosion()
    {
        var go = new GameObject();
        var sr = go.AddComponent<SpriteRenderer>();
        var col = go.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.3f;
        col.offset = new Vector2(0, -0.15f);
        
        var poison = go.AddComponent<PoisonObject>();
        col.enabled = false;
        sr.sprite = SpriteToSpawn;
        go.transform.position = new Vector3(Random.Range(TopLeft.position.x, DownRight.position.x),
            Random.Range(DownRight.position.y, TopLeft.position.y), 1f);

        go.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

        go.transform.parent = _spawnedObjectsParent;
        go.name = SpriteToSpawn.name;
    }
}
