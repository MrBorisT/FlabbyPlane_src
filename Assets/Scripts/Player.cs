using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        GameStart,
        GameOverSequence,
        GameOver,
        GameReset
    }
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }


    const string GAMEOVER = "GameOver";
    private GameInput gameInput;
    private bool isPlaying;
    private bool isDead;
    private Vector3 initPosition;
    private Quaternion initRotation;

    private Rigidbody2D rb;
    private float initGravityScale;

    [SerializeField] GameObject puffs;
    [Header("Sounds")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip collisionClip;
    [SerializeField] AudioClip[] wooshClips;
    [SerializeField] float playerVolume = 0.3f;

    [Header("Rotation params")]
    [SerializeField] float minVelocityForRotation = -10f;
    [SerializeField] float maxVelocityForRotation = 10f;
    [SerializeField] float xOffsetForRotation = 5f;

    [Header("Jump params")]
    [SerializeField] Vector2 jumpVelocity;
    [SerializeField] float downGravityScale = 4f;

    [Header("Game Over sequence timers")]
    [SerializeField] float gameOverDelay = 1f;


    private void Awake()
    {
        gameInput = FindObjectOfType<GameInput>();
        rb = GetComponent<Rigidbody2D>();
        initGravityScale = rb.gravityScale;
    }

    private void Start()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        Init();
    }

    private void Init()
    {
        transform.position = initPosition;
        transform.rotation = initRotation;
        rb.angularVelocity = 0f;
        rb.velocity = Vector2.zero;
        isDead = false;
    }

    private void GameInput_OnJumpAction(object sender, System.EventArgs e)
    {
        if (isPlaying)
        {
            Jump();
        }
        else
        {
            if (isDead)
            {
                Init();
                transform.position = initPosition;
                transform.rotation = initRotation;
                rb.angularVelocity = 0f;
                rb.velocity = Vector2.zero;
                isDead = false;
                OnStateChange?.Invoke(this, new OnStateChangeEventArgs{ 
                    state = State.GameReset
                });
                puffs.SetActive(true);
            }
            else
            {
                OnStateChange.Invoke(this, new OnStateChangeEventArgs
                {
                    state = State.GameStart
                });
                isPlaying = true;
                Jump();
            }
        }
    }

    private void Jump()
    {
        AudioSource.PlayClipAtPoint(wooshClips[UnityEngine.Random.Range(0, wooshClips.Length)], Camera.main.transform.position, playerVolume);
        rb.velocity = jumpVelocity;
    }

    private void Update()
    {
        if (isPlaying)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        float clampedVelocity = Mathf.Clamp(rb.velocity.y, minVelocityForRotation, maxVelocityForRotation);
        Vector3 lookAtVector = new Vector3(xOffsetForRotation, clampedVelocity, 0);
        transform.right = lookAtVector;
    }

    private void FixedUpdate()
    {
        if (!isPlaying)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            return;
        }
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = downGravityScale;
        }
        else
        {
            rb.gravityScale = initGravityScale;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GAMEOVER)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(collisionClip, Camera.main.transform.position, playerVolume);
        gameInput.OnJumpAction -= GameInput_OnJumpAction;
        isDead = true;
        isPlaying = false;
        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
        {
            state = State.GameOverSequence
        });
        StartCoroutine(StartGameOverSequence());
    }

    private IEnumerator StartGameOverSequence()
    {
        yield return new WaitForSeconds(gameOverDelay);
        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
        {
            state = State.GameOver
        });
        puffs.SetActive(false);
        AudioSource.PlayClipAtPoint(explosionClip, Camera.main.transform.position);
        gameInput.OnJumpAction += GameInput_OnJumpAction;
    }
}
