using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //public float movementForce, jumpForce;
    //public Rigidbody rigidBody;
    public CharacterController controller;

    [Header("Control Properties")]
    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHeight = 3.0f;
    public Vector3 velocity;

    [Header("Ground Detection Properties")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundRadius = 0.5f;
    public bool isGrounded;

    [Header("MiniMap")]
    public GameObject miniMap;

    [Header("Player Sounds")]
    public AudioSource jumpSound;
    public AudioSource hitSound;

    [Header("Healthbar")]
    public HealthBarScreenSpaceController healthBar;

    [Header("Player Abilities")]
    [Range(0, 100)]
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * maxSpeed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            jumpSound.Play();
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(!miniMap.activeInHierarchy);
        }

        //if (isGrounded)
        //{
        //    if (Input.GetAxisRaw("Horizontal") > 0)
        //    {
        //        rigidBody.AddForce(Vector3.right * movementForce);
        //    }

        //    if (Input.GetAxisRaw("Horizontal") < 0)
        //    {
        //        rigidBody.AddForce(Vector3.left * movementForce);
        //    }

        //    if (Input.GetAxisRaw("Vertical") > 0)
        //    {
        //        rigidBody.AddForce(Vector3.forward * movementForce);
        //    }

        //    if (Input.GetAxisRaw("Vertical") < 0)
        //    {
        //        rigidBody.AddForce(Vector3.back * movementForce);
        //    }

        //    if (Input.GetAxisRaw("Jump") > 0)
        //    {
        //        rigidBody.AddForce(Vector3.up * jumpForce);
        //    }
        //}
    }

    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //void OnCollisionStay(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Invoke("PlayHitSound", 1.0f);
        healthBar.TakeDamage(damage);
        if (health < 0)
        {
            health = 0;
        }
    }

    public void PlayHitSound()
    {
        hitSound.Play();
    }
}
