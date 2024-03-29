﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerWon;
    public Animator animator;
    [SerializeField]
    public static int power = 1;
    [SerializeField]
    private float speedIncrease = 0.3f;
    public BombSpawner bombSpawner;
    public static int bombs;
    private float speed = 1;
    private Rigidbody2D rb2d;
    private Vector2 movement;

    public PlayerController thePlayer;

    void OnEnable() 
    {
        GameManager.OnGameStarted += StartGame;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= StartGame;
    }
    void StartGame()
    {
        // Enable control
        Time.timeScale = 1;
    }

    void StopGame()
    {
        // Freeze time
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        // Freeze time
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.IsInputEnabled)
        {
            return;
        }
        movement = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))).normalized;
        setAnimator(movement);
    }

    void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        rb2d.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }

    void setAnimator(Vector2 direction)
    {
        float horizontal = movement[0];
        float vertical = movement[1];
        animator.SetFloat("speed", movement.magnitude); // magnitude is slow
        if (horizontal < 0)
        {
            animator.SetInteger("direction", 2);
        }
        else if (vertical < 0)
        {
            animator.SetInteger("direction", 3);
        }
        else if (vertical > 0)
        {
            animator.SetInteger("direction", 1);
        }
        else
        {
            animator.SetInteger("direction", 0);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag)
        {
            case "BombPowerUp":
                Destroy(col.gameObject);
                bombSpawner.incrementBomb();
                break;
            case "SpeedPowerUp":
                Destroy(col.gameObject);
                speed += speedIncrease;
                break;
            case "ShieldPowerUp":
                Destroy(col.gameObject);
                break;
            case "BlastPowerUp":
                Destroy(col.gameObject);
                power += 1;
                break;
            case "DeadZone":
                StopGame();
                OnPlayerDied(); // event sent to game manager
                break;
            case "EndZone":
                StopGame();
                OnPlayerWon(); // event sent to game manager
                break;
            default:
                break;
        }
    }
}
