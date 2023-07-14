using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Script for spawning game objects at given positions without intersecting with other objects.
/// </summary>
public class FixedPositionSpawner : MonoBehaviour
{
    [Header("Top left point of spawning area")]
    [SerializeField]
    private Transform TopLeft;
    
    [Header("Bottom right point of spawning area")]
    [SerializeField]
    private Transform BottomRight;
    
    [Header("Spawned game object")]
    [SerializeField]
    private GameObject GameObjectPrefab;
    
    [Header("Interval at each objects are spawned")]
    [SerializeField]
    private float SpawnInterval;

    private float spawnTimer = 0.0f;

    private GameObject player;

    [Header("Initial number of spawned objects")]
    [SerializeField]
    private float InitialSpawnedNumber;

    private GameObject parent;
    
    
    private float levelCoef = 1.0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        parent = GameObject.FindGameObjectWithTag("InstantiatedObjectsParent");
    }

    void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] / 12f;
        if (levelCoef >= 2.5f)
        {
            levelCoef = 2.5f;
        }
        SpawnInterval /= levelCoef;
        for (int i = 0; i < InitialSpawnedNumber; i ++)
        {
            Spawn();
        }
    }
    
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnInterval)
        {
            Spawn();
            spawnTimer = 0;
        }
    }

    private void Spawn()
    {
        Vector2 playerPos = player.transform.position;
        float randomX = Random.Range((int)TopLeft.position.x, (int)BottomRight.position.x) + 0.5f;
        float randomY = Random.Range((int)BottomRight.position.y, (int)TopLeft.position.y) + 0.5f;
        float distanceFromPlayer = Vector2.Distance(playerPos, new Vector2(randomX, randomY));
        float distanceFromBoss = Vector2.Distance(new Vector2(0, 0), new Vector2(randomX, randomY));
        Collider2D Collision = Physics2D.OverlapCircle(new Vector2(randomX,randomY), 0.9f, LayerMask.GetMask("SpawnedObject"));
        int attempts = 0;
        while (distanceFromPlayer <= 6 || Collision || distanceFromBoss <= 4)
        {
            attempts++;
            if (attempts >= 50)
                return;
            randomX = Random.Range((int)TopLeft.position.x, (int)BottomRight.position.x) + 0.5f;
            randomY = Random.Range((int)BottomRight.position.y, (int)TopLeft.position.y) + 0.5f;
            distanceFromPlayer = Vector2.Distance(playerPos, new Vector2(randomX, randomY));
        }
        Vector3 pos = new Vector3(randomX, randomY, 0);
        GameObject SpawnedObject = Instantiate(GameObjectPrefab, pos, Quaternion.identity);
        SpawnedObject.transform.SetParent(parent.transform);
        StartCoroutine(Appear(1, SpawnedObject));
    }
    
    private IEnumerator Appear(float tweeningDuration, GameObject ob)
    {
        ob.GetComponent<Collider2D>().enabled = false;
        Vector3 objectScale = ob.transform.localScale;
        for (float t = 0; t < 1; t += Time.deltaTime / tweeningDuration)
        {
            ob.transform.localScale = objectScale * Mathf.SmoothStep(0, 1, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        ob.GetComponent<Collider2D>().enabled = true;
    }
}
