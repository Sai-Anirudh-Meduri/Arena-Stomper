using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f; // I might adjust this to be faster due to gravity value
    public float runSpeed = 8f; // I might adjust this to be faster due to gravity value
    public float jumpForce = 2f; // Test this by manipulating gravity value to see if it affects when jumping
    public float gravity = -10f; // For armor feeling because the player is a knight
    public float rotationSpeed = 100f; // Might allow player to manipulate this as a setting feature

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded; // Check if the player is on the ground

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // For every frame
        MovePlayer();
        HandleAnimations();
    }

    void MovePlayer()
    {
        // Check if the player is on the ground and adjust velocity
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Mouse rotation by getting mouse input
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);

        // Movement input by WASD keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Move relative to player direction
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Running check and implements a current speed to decide between walk and run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Apply movement
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jumping by pressing space and ground check
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAnimations()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Walking
        animator.SetBool("Walk", isMoving);

        // Running
        animator.SetBool("Run", isMoving && isRunning);

        // Blocking
        animator.SetBool("Block", Input.GetKey(KeyCode.Q));

        // Attacking
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }
}
