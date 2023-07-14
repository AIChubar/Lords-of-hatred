using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for spawning game objects at given positions without intersecting with other objects.
/// </summary>
public class GameObjectSpawner : MonoBehaviour
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
    
    private GameObject parent;

    [Header("How long object exists in seconds")]
    [SerializeField]
    private float ObjectLifetime;

    [Header("Time between each spawn")]
    [SerializeField]
    private float SpawnInterval;

    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    [SerializeField]
    private Sound SoundAppearing;
    
    float spawnTimer = 0.0f;
    
    private float levelCoef = 1.0f;
    
    private void Awake()
    {
        parent = GameObject.FindGameObjectWithTag("InstantiatedObjectsParent");
    }
    private void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] / 12f;
        if (levelCoef >= 2.5f)
            levelCoef = 2.5f;
        SpawnInterval /= levelCoef;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer > SpawnInterval)
        {
            spawnTimer = 0;
            SpawnObject();
        }
        
    }
    

    private void SpawnObject()
    {
        Vector3 pos = new Vector3(Random.Range(TopLeft.position.x, BottomRight.position.x),
            Random.Range(BottomRight.position.y, TopLeft.position.y), 1f);

        Collider2D Collision = Physics2D.OverlapCircle(pos, 2f, LayerMask.GetMask("SpawnedObject"));
        int attempts = 0;
        while (Collision)
        {
            attempts += 1;
            if (attempts > 50)
            {
                break;
            }
            pos = new Vector3(Random.Range(TopLeft.position.x, BottomRight.position.x),
                Random.Range(BottomRight.position.y, TopLeft.position.y), 1f);
            Collision = Physics2D.OverlapCircle(pos, 1.5f, LayerMask.GetMask("SpawnedObject"));
        }

        var ob = Instantiate(GameObjectPrefab, pos, Quaternion.identity);

        ob.transform.SetParent(parent.transform);
        
        StartCoroutine(ObjectLife(2, ob, ObjectLifetime ));
    }
    
    private IEnumerator ObjectLife(float tweeningDuration, GameObject ob, float objectLifeTime)
    {
        ob.GetComponent<Collider2D>().enabled = false;
        AudioManager.instance.Play(SoundAppearing);
        Vector3 objectScale = ob.transform.localScale;
        for (float t = 0; t < 1; t += Time.deltaTime / tweeningDuration)
        {
            ob.transform.localScale = objectScale * Mathf.SmoothStep(0, 1, t);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);

        ob.GetComponent<Collider2D>().enabled = true;
        
        yield return new WaitForSeconds(objectLifeTime);

        for (float t = 0; t < 1; t += Time.deltaTime / tweeningDuration)
        {
            ob.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        
        Destroy(ob);
    }
}
