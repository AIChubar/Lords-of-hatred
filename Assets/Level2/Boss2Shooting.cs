using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Script for level two boss shooting logic.
/// </summary>
public class Boss2Shooting : MonoBehaviour
{
    [Header("Missile game object prefab")]
    public GameObject MisslePrefab;


    [Header("Base missile speed")]
    [SerializeField]
    private float MissileSpeed = 10f;

    [Header("Base missile number per attack")]
    [SerializeField]
    private int MissileNumber = 6;

    private GameObject Parent;

    private float AngleBetweenMissiles;
    
    private float levelCoef = 1.0f;
    
    
    private void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[1] / 12f;
        Parent = GameObject.FindGameObjectWithTag("InstantiatedObjectsParent");
        MissileNumber = (int)(MissileNumber * levelCoef);
        Physics2D.IgnoreLayerCollision(MisslePrefab.layer, MisslePrefab.layer, true);
        AngleBetweenMissiles = 360f / MissileNumber;
    }

    public void Shoot()
    {
        float startingAngleShift = Random.Range(0, AngleBetweenMissiles/2f);
        for (int i = 0; i < MissileNumber; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, -180f + startingAngleShift + i * AngleBetweenMissiles);
            GameObject missile = Instantiate(MisslePrefab, transform.position, rot);
            missile.transform.SetParent(Parent.transform);
            missile.GetComponent<Boss2Missile>().SetSpeed(MissileSpeed);
            if (levelCoef <= 2.5f)
                missile.transform.localScale *= levelCoef;
            else
                missile.transform.localScale *= 2.5f;
            
        }
    }
}
