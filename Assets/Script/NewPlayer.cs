using System.Collections;
using UnityEngine;
using System;

public class NewPlayer : UnityEngine.MonoBehaviour
{
    private Vector3 originalSize;
    [Range(0,10)] public int health, maxHealth;
    [Range(0,1)]public float energy,MaxEnergy;
    public event Action<int> OnHealthChange;
    private bool death;

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 10f;
    private Vector2 direction;
    [SerializeField] private bool facingRight = true;
    private float directionX;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Physics")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float fallMultiplier = 5f;

    [Header("Collision")]
    private bool onGround;
    [SerializeField] private float groundLength = 0.6f;
    [SerializeField] private Vector3 colliderOffset;

    [Header("Audio")]
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip gemSound;

    [Header("Mision")]
    [SerializeField]private int score;
    [SerializeField] private float timeRestartLevel;
    private int haveScore;
    private bool dead;

    private static NewPlayer instance;
	public static NewPlayer Instance
	{
		get
		{
			if (instance == null) instance = GameObject.FindObjectOfType<NewPlayer>();
			return instance;
		}
	}

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        score = Door.Instance.score;
        originalSize = gameObject.transform.lossyScale;
    }

    private void Update()
    {
        bool wasOnGround = onGround;
        onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer))&&!death;

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        animator.SetBool("onGround", onGround);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    }

    public void ControllerButton(float direction)
    {
        if(!dead)
        directionX = direction;
    }

    private void FixedUpdate()
    {
        MoveCharacter(directionX);
        ModifyPhysics(); 
    }

    private void MoveCharacter(float horizontal)
    {
        if (!death)
        {
            rb.AddForce(Vector2.right * horizontal * moveSpeed);

            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
            {
                Flip();
            }

            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }

            if (Math.Abs(rb.velocity.x) > 0.2 && onGround && !audioSource.isPlaying)
             PlaySoundSFX(stepSound);
           
            double velocityX = Math.Round(rb.velocity.x, 1);
            animator.SetFloat("horizontal", Mathf.Abs((float)velocityX));  //створти і підключити анімації  
            animator.SetFloat("vertical", rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "gem")
        {
            haveScore++;
            Destroy(collision.gameObject);
            PlaySoundSFX(gemSound);
            if (haveScore == score)
            {
                Door.Instance.OpenDoor(); 
            }
        }

        if (collision.gameObject.tag == "Finish" && haveScore == score)
        {
            animator.SetBool("Door", true);
            StartCoroutine(LoadedLevel(Application.loadedLevel+1, timeRestartLevel+1f));
            dead = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cannonball")
        {
            Destroy(collision.gameObject);
            TakeDamege(1);
        }
    }

    private void ModifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier/1.6f;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    public void TakeDamege(int damege)
    {
        health -= damege;
        if (OnHealthChange != null)
        {
            OnHealthChange.Invoke(health);
        }
        animator.SetBool("TakeDamege", true);
        StartCoroutine(LoadedLevel(Application.loadedLevel,timeRestartLevel));
        dead = true;
    }


    IEnumerator LoadedLevel(int indexLevel,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.LoadLevel(indexLevel);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        if (directionX > 0) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }

    private void PlaySoundSFX(AudioClip audioClip)
    {
        audioSource.pitch = (UnityEngine.Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(audioClip);
    }

    private void PlaySoundSFX(AudioClip audioClip, float minPitch, float maxPitch)
    {
        audioSource.pitch = (UnityEngine.Random.Range(minPitch, maxPitch));
        audioSource.PlayOneShot(audioClip);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
