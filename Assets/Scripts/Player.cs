using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool canJump = true;
    private Animator controller;
    private int facingRight = -1;
    private SpriteRenderer spriteRenderer;
    /* PREFABS */
    [SerializeField]
    private GameObject swordCollider;
    /* EDITABLE VARIABLES */
    [SerializeField]
    public float jumpVelocity = 5f;
    [SerializeField]
    public float movementSpeed = 5f;
    private float slashDuration = 0.3f;
    private int lives = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            canJump = false;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            controller.Play("Slash");
            controller.SetBool("isHitting", true);
            StartCoroutine(Slash());
        }

        CalculateMovement();
    }

    IEnumerator Slash()
    {
        swordCollider.SetActive(true);
        float timeElapsed = 0;
        Quaternion startRotation = swordCollider.transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, facingRight * 120);

        while (timeElapsed < slashDuration)
        {
            swordCollider.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slashDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        swordCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        controller.SetBool("isHitting", false);
        swordCollider.SetActive(false);
    }


    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * movementSpeed * Time.deltaTime);
        Debug.Log(horizontalInput);
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            controller.SetBool("isMoving", true);
            facingRight = -1;
        } else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            controller.SetBool("isMoving", true);
            facingRight = 1;
        } else
        {
            controller.SetBool("isMoving", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Floor")
        {
            canJump = true;
        }

        if (other.transform.tag == "Spikes")
        {
            TakeDamage(lives);
        }
        
        if (other.transform.tag == "Enemy")
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        lives -= damage;
        StartCoroutine(DamageFlicker());
        if (lives < 1)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator DamageFlicker()
    {
        spriteRenderer.color = new Color(1f, 0.136f, 0.136f, 0.5f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.white;
    }
}
