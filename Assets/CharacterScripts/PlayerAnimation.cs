using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script responsible for player behaviour.
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private Vector2 movement;
    
    private Rigidbody2D rb;
    
    private bool colliding = false;

    private bool damageAnimation = false;

    [Header("Duration of damaged animation.")]
    [SerializeField]
    private float AnimationDuration;
    
    private SpriteRenderer sprite;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SoundDamageReceived;

    [HideInInspector]
    public List<DamageableObject> collidingObjects;

    //private CharacterController controller;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        //controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding && !damageAnimation)
        {
            if (!GameManager.gameManager.Character.healthSystem.Damage(collidingObjects[0].damage))
            {
                colliding = false;
                GameManager.gameManager.PauseMenu.GameOver();
                GetComponent<Collider2D>().enabled = false;
                movement.x = 0;
                movement.y = 0;
                return;
            }
            
            StartCoroutine(DamageReceived(AnimationDuration));
            
        }
        
    }
    
    private void FixedUpdate()
    {
        movement = playerInput.Player.Move.ReadValue<Vector2>();
        rb.MovePosition(rb.position + movement * (GameManager.gameManager.Character.MovementSpeed.Value * Time.fixedDeltaTime));

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            colliding = true;
            collidingObjects.Add(collidingObject);
            collidingObjects.Sort((o1,o2)=>o1.damage.CompareTo(o2.damage));
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            collidingObjects.Remove(collidingObject);
            if (collidingObjects.Count == 0)
                colliding = false;
        }
       
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.TryGetComponent<DamageableObject>(out var collidingObject) && !colliding && !damageAnimation)
        {
            if (!GameManager.gameManager.Character.healthSystem.Damage(collidingObject.damage))
            {
                colliding = false;
                GameManager.gameManager.PauseMenu.GameOver();
                GetComponent<Collider2D>().enabled = false;
                movement.x = 0;
                movement.y = 0;
                return;
            }
            StartCoroutine(DamageReceived(AnimationDuration));
        }
    }
    
    
    private IEnumerator DamageReceived(float duration)
    {
        damageAnimation = true;
        AudioManager.instance.Play(SoundDamageReceived);
        
        for (int j = 0; j < 10; j++)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / duration * 20f)
            {
                sprite.color = new Color(1, Mathf.SmoothStep(1, 0, t), Mathf.SmoothStep(1, 0, t));
                yield return null;
            }
        
            for (float t = 0; t < 1; t += Time.deltaTime / duration * 20f)
            {
                sprite.color = new Color(1, Mathf.SmoothStep(0, 1, t), Mathf.SmoothStep(0, 1, t));
                yield return null;
            }
        }

        sprite.color = Color.white;
        damageAnimation = false;

    }
}
