using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player shooting logic.
/// </summary>
public class Shooting : MonoBehaviour
{
    [Header("Point from which missile is being shot")]
    [SerializeField]
    private Transform FirePoint;
    
    [Header("Missile game object prefab")]
    [SerializeField]
    public GameObject MisslePrefab;
    
    [Header("Character object transform to calculate missile direction")]
    [SerializeField]
    public Transform CharTransform;
    
    private Camera mainCam;
    private Vector3 mousPos;

    private float missileSpeed;

    private float shootingTimer = 0.0f;

    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SoundShooting;
    
    private GameObject instantiatedObjectsParent;

    private void Start()
    {
        instantiatedObjectsParent = GameObject.FindGameObjectWithTag("InstantiatedObjectsParent");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        missileSpeed = GameManager.gameManager.Character.MissileSpeed.Value;
    }
    
    private void Update()
    {
        if (GameManager.gameManager.PauseMenu.pauseMode != PauseMode.UnPaused)
        {
            return;
        }
        mousPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousPos - transform.position;
        float rotZ = MathF.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        if (SceneManager.GetActiveScene().buildIndex == 2)
            rotZ = 0f;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        
        shootingTimer += Time.deltaTime;
        if (Input.GetButton("Fire1"))
        {
            if (shootingTimer >= GameManager.gameManager.Character.ShootingDelay.Value)
            {
                Shoot();
                shootingTimer = 0.0f;
            }
        }
        
    }

    private void Shoot()
    {
        AudioManager.instance.Play(SoundShooting);
        GameObject missile = Instantiate(MisslePrefab, FirePoint.position, Quaternion.identity);
        missile.transform.SetParent(instantiatedObjectsParent.transform);
        missile.tag = "Missile";
        Vector3 direction = FirePoint.position - CharTransform.position;
        Vector3 rotation = FirePoint.position - CharTransform.position;
        Vector2 velocity = new Vector2(direction.x, direction.y).normalized * missileSpeed;
        Quaternion rot = Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
        MissileObject missileComp = missile.GetComponent<MissileObject>();
        missileComp.SetVelocityRot(velocity, rot);

    }
}
