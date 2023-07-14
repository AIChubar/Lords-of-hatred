using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script Containing logic for bees on the first level.
/// </summary>
public class BeesMovement : MonoBehaviour
{
    [Header("Bees basic movement speed")]
    [SerializeField]
    private float moveSpeed = 2f;

    [Header("Bees rigid body")]
    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 movement;

    private Vector2 DefaultPos;
    
    private float levelCoef = 1.0f;

    void Start()
    {
        DefaultPos = transform.position;
        levelCoef += GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] / 12f;
        if (levelCoef >= 2.5f)
            levelCoef = 2.5f;
        moveSpeed *= levelCoef;
    }
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
