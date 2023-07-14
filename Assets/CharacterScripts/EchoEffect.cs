using UnityEngine;

/// <summary>
/// Effects that adds trail of sprites clones behind moving object.
/// </summary>
public class EchoEffect : MonoBehaviour
{
    
    private float timeBtwSpawns;

    [Header("Interval when copies are starting to spawn after object initiation")]
    [SerializeField]
    private float StartTimeBtwSpawns;

    [Header("Sprite prefab that will be spawned")]
    [SerializeField]
    private GameObject Echo;

    private GameObject instantiatedObjectsParent;
    
    void Start()
    {
        timeBtwSpawns -= timeBtwSpawns * GameManager.gameManager.Character.statLevels.MovementSpeed/5f;
        instantiatedObjectsParent = GameObject.FindGameObjectWithTag("InstantiatedObjectsParent");
    }

    void Update()
    {
        if (timeBtwSpawns <= 0)
        {
            GameObject instance = Instantiate(Echo, transform.position, Quaternion.identity);
            instance.transform.SetParent(instantiatedObjectsParent.transform);
            Destroy(instance, 6f);
            timeBtwSpawns = StartTimeBtwSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
