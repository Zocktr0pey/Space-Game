using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float maxVel = 10f;
    [SerializeField] private float counterForceFactor = 0.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float pushForceFactor = 1f;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private bool isGrounded;

    private InputManager inputManager;
    private AudioManager audioManager;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = InputManager.Instance;
        audioManager = AudioManager.Instance;
        rb = GetComponent<Rigidbody>();
        isGrounded = false;

        // Init stats
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        GroundedCheck();

        velocity.y += gravity * Time.deltaTime;

        Movement();

        // Schiessen!
        // Einzelschuss
        if (inputManager.GetSingleFire())
        {
            Debug.Log("Pew Pew");
            // Hier Einzeschusswaffen.shoot()
        }

        // Dauerfeuer (wird bei JEDEM Frame getriggert)
        if (inputManager.GetContinuousFire())
        {
            // Hier Dauerfeuerwaffen.shoot()
        }
    }

    public void GroundedCheck()
    {
        isGrounded = Physics.Raycast(rb.position + new Vector3(0, 0.05f, 0), Vector3.down, out RaycastHit hit, groundDistance);
        Debug.DrawLine(rb.position, hit.point);
    }

    public void Movement()
    {
        if (!isGrounded) { return; }

        moveInput = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);

        Debug.Log(isGrounded);

        // CounterMovement
        if (moveInput.x == 0)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * transform.right * -localVelocity.x * counterForceFactor);
        }
        if (moveInput.y == 0)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * transform.forward * -localVelocity.z * counterForceFactor);
        }

        // maxSpeed check
        if (Mathf.Abs(localVelocity.x) > maxVel) { move.x = 0; }
        if (Mathf.Abs(localVelocity.z) > maxVel) { move.z = 0; }

        move = transform.TransformDirection(move); //
        rb.AddForce(move * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage) 
    { 
        currentHealth -= damage;
        audioManager.PlayerDamage();

        if (currentHealth <= 0)
        {
            //audioManager.PlayerDeath()
            //gameManager.GameOver();
        }
    }
}
