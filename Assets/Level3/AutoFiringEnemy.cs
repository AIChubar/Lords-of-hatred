using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for the enemy that is firing towards player if it is in the shooting range.
/// </summary>
public class AutoFiringEnemy : MonoBehaviour
{
    [Header("Missile game object prefab")]
    [SerializeField]
    private GameObject MisslePrefab;
    
    [Header("Enemy missile speed")]
    [SerializeField]
    private float MissileSpeed;

    [Header("Enemy shooting delay")]
    [SerializeField]
    private float ShootingDelay;

    private float shootingTimer = -0.5f;

    private bool colliding = false;

    private PlayerAnimation player;

    [Header("Area on which a player is detected to trigger a shot")]
    [SerializeField]
    private Vector2 DetectionArea;
    
    private float levelCoef = 1.0f;

    void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] / 12f;
        if (levelCoef >= 2.5f)
        {
            levelCoef = 2.5f;
        }

        MissileSpeed *= levelCoef;
        ShootingDelay /= levelCoef;
        BoxCollider2D detectionCollider = gameObject.AddComponent<BoxCollider2D>();
        detectionCollider.size = DetectionArea;
        detectionCollider.isTrigger = true;
    }
    
    void Update()
    {
        shootingTimer += Time.deltaTime;
        if (colliding && shootingTimer > ShootingDelay)
        {
            Shoot(player.transform.position);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<PlayerAnimation>(out PlayerAnimation player))
        {
            colliding = true;
            this.player = player;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<PlayerAnimation>(out PlayerAnimation player))
        {
            colliding = false;
        }
    }

    private void Shoot(Vector3 heroPosition)
    {
        shootingTimer = 0.0f;
        GameObject missile = Instantiate(MisslePrefab, transform.position, Quaternion.identity);
        missile.transform.SetParent(GameObject.FindGameObjectWithTag("InstantiatedObjectsParent").transform);
        Vector3 direction = heroPosition - transform.position;
        Vector2 velocity = new Vector2(direction.x, direction.y).normalized * MissileSpeed;
        missile.GetComponent<StatueMissile>().SetVelocity(velocity);;
    }
    
}
