using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Script containing behaviour of the first level boss.
/// </summary>
public class Boss1Behaviour : Boss
{
    [Header("Boss movement speed")]
    [SerializeField]
    private float MoveSpeed;
    
    private Rigidbody2D rb;

    private Vector2 Movement;

    private float timer = 0.0f;

    private bool soundPlayed = false;
    
    private bool movingUp = true;
    
    private bool movingLeft = true;
    
    private float nextCharge;

    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SoundDash;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();

    }

    protected override void Start()
    {
        base.Start();

        nextCharge = Random.Range(5f, 10f) / levelCoef;
    }
    
    void Update()
    {
        if (dying)
        {
            Movement.x = 0;
            Movement.y = 0;
            return;
        }

        timer += Time.deltaTime;
        
        if (rb.position.y > 3.5f)
            movingUp = false;
        if (rb.position.y < -3.0f)
            movingUp = true;

        if (rb.position.x < -6.0f)
            movingLeft = false;
        if (rb.position.x > 5.5f)
        {
            soundPlayed = false;
            movingLeft = true;
            timer = 0.0f;
            nextCharge = Random.Range(5f, 10f) / levelCoef;
            Movement.x = 0.0f;
        }
        
        if (movingUp)
            Movement.y = 0.6f * levelCoef;
        else
            Movement.y = -0.6f * levelCoef;

        if (timer >= nextCharge - 1)
            Movement.y = 0.0f;
        if (timer >= nextCharge)
        {
            if (!soundPlayed)
            {
                AudioManager.instance.Play(SoundDash);
                soundPlayed = true;
            }
            if (movingLeft)
                Movement.x = -8.0f * levelCoef;
            else
                Movement.x = 4.0f * levelCoef;
        }
            
    }
    

    private void FixedUpdate()
    {
        var position = rb.position;
        var coef = MoveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = new Vector2(position.x + Movement.x * coef,
            position.y + Movement.y * coef);
        
        if (rb.position.x > 5.5f)
            newPos.x = 5.5f;
        
        rb.MovePosition(newPos);
    }
}
