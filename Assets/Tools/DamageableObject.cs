using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script that is attached to any enemy or object that can do damage.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DamageableObject : MonoBehaviour
{
    [Tooltip("Base object damage")]
    [SerializeField]
    public float damage;
    
    private float levelCoef = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[SceneManager.GetActiveScene().buildIndex - 2] / 6f;
        if (levelCoef >= 3)
        {
            float overFixedCoef = levelCoef - 3f;
            levelCoef = 3f + overFixedCoef * 2;
        }
        damage *= levelCoef;
    }
    
}
