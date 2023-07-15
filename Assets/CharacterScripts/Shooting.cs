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
    private Vector3 pointerPos;

    private float missileSpeed;

    private float shootingTimer = 0.0f;

    private float shootingReactionDelay = 0.2f;

    private float rotZ;

    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SoundShooting;
    
    private GameObject instantiatedObjectsParent;
    
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

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

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            pointerPos = mainCam.ScreenToWorldPoint(playerInput.Player.OrbPos.ReadValue<Vector2>());
            Vector3 rotation = pointerPos - transform.position;
            rotZ = MathF.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            if (SceneManager.GetActiveScene().buildIndex == 2)
                rotZ = 0f;
        }
        else
        {
            pointerPos = (Vector3)playerInput.Player.OrbPos.ReadValue<Vector2>() + CharTransform.position;
        }
        
        
        shootingTimer += Time.deltaTime;
        if (playerInput.Player.Shoot.IsPressed())
        {
            shootingReactionDelay -= Time.deltaTime;
            if (shootingTimer >= GameManager.gameManager.Character.ShootingDelay.Value && (shootingReactionDelay < 0f || SystemInfo.deviceType == DeviceType.Desktop))
            {
                Shoot();
                shootingTimer = 0.0f;
            }

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                Vector3 rotation = pointerPos - transform.position;
                rotZ = MathF.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                if (SceneManager.GetActiveScene().buildIndex == 2)
                    rotZ = 0f;
            }
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (playerInput.Player.Shoot.WasReleasedThisFrame())
            {
                shootingReactionDelay = 0.2f;
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        
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
