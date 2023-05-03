using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

//Unused
public class AutoFiringTileMapScript : MonoBehaviour
{
    public GameObject GameObjectPrefab;

    public SpawningTile Tile;
    
    private Tilemap SpawningArea;

    public float SpawnInterval;

    private float SpawnTimer = 0.0f;

    private GameObject Player;

    public float InitialSpawnedNumber;
    
    private List<GameObject> SpawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        SpawningArea = GetComponent<Tilemap>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        for (int i = 0; i < InitialSpawnedNumber; i ++)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer >= SpawnInterval)
        {
            int randomX = Random.Range(SpawningArea.cellBounds.xMin, SpawningArea.cellBounds.xMax);
            int randomY = Random.Range(SpawningArea.cellBounds.yMin, SpawningArea.cellBounds.yMax);
            while (SpawningArea.GetTile(new Vector3Int(randomX, randomY, 0)) is not null &&
                   Vector2.Distance(new Vector2(0,0), new Vector2(randomX, randomY)) <= 6)
            {
                randomX = Random.Range(SpawningArea.cellBounds.xMin, SpawningArea.cellBounds.xMax);
                randomY = Random.Range(SpawningArea.cellBounds.yMin, SpawningArea.cellBounds.yMax);
            }

            SpawnTimer = 0;
            SpawningArea.SetTile(new Vector3Int(randomX, randomY, 0), Tile);
        }
    }

    private void Spawn()
    {
        Vector2 playerPos = Player.transform.position;
        int randomX = Random.Range(SpawningArea.cellBounds.xMin, SpawningArea.cellBounds.xMax);
        int randomY = Random.Range(SpawningArea.cellBounds.yMin, SpawningArea.cellBounds.yMax);
        float distance = Vector2.Distance(playerPos, new Vector2(randomX, randomY));
        while (SpawningArea.GetTile(new Vector3Int(randomX, randomY, 0)) is not null ||
               distance <= 7)
        {
            randomX = Random.Range(SpawningArea.cellBounds.xMin, SpawningArea.cellBounds.xMax);
            randomY = Random.Range(SpawningArea.cellBounds.yMin, SpawningArea.cellBounds.yMax);
            distance = Vector2.Distance(playerPos, new Vector2(randomX, randomY));
        }
        Vector3 centerPosition = new Vector3(randomX + 0.5f, randomY + 0.5f, 0);
        GameObject SpawnedObject = Instantiate(GameObjectPrefab, centerPosition, Quaternion.identity);
        SpawnedObject.transform.SetParent(GameObject.FindGameObjectWithTag("InstantiatedObjectsParent").transform);
        SpawnedObjects.Add(SpawnedObject);
        SpawnTimer = 0;
        SpawningArea.SetTile(new Vector3Int(randomX, randomY, 0), Tile);
    }
    
}
