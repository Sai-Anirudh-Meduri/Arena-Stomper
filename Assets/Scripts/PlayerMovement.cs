using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 2f;
    public float gravity = -10f;
    public float rotationSpeed = 150f;

    [Header("Combat Settings")]
    public float attackDuration = 1f;

    [Header("Player Combat")]
    [SerializeField] private float playerAttackDamage = 25f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private LayerMask enemyLayer;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isAttacking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (Time.timeScale != 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        MovePlayer();
        HandleAnimations();
    }

    void MovePlayer()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotationSpeed);

        Vector2 input = GetMovementInput();

        float horizontal = input.x;
        float vertical = input.y;

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAnimations()
    {
        Vector2 input = GetMovementInput();

        float horizontal = input.x;
        float vertical = input.y;

        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        animator.SetBool("Walk", isMoving);
        animator.SetBool("Run", isMoving && isRunning);
        animator.SetBool("Block", Input.GetKey(KeyCode.Q));

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartAttack();
        }
    }

    Vector2 GetMovementInput()
    {
        int controlScheme = PlayerPrefs.GetInt("ControlScheme", 0);

        float horizontal = 0f;
        float vertical = 0f;

        if (controlScheme == 0)
        {
            // WASD controls
            if (Input.GetKey(KeyCode.A)) horizontal = -1f;
            if (Input.GetKey(KeyCode.D)) horizontal = 1f;
            if (Input.GetKey(KeyCode.W)) vertical = 1f;
            if (Input.GetKey(KeyCode.S)) vertical = -1f;
        }
        else
        {
            // Arrow key controls
            if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) vertical = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) vertical = -1f;
        }

        return new Vector2(horizontal, vertical).normalized;
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        DealDamageToWolf();

        Invoke(nameof(EndAttack), attackDuration);
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    public bool IsBlocking()
    {
        return Input.GetKey(KeyCode.Q);
    }

    void DealDamageToWolf()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position + transform.forward * 1.5f,
            attackRange,
            enemyLayer
        );

        Debug.Log("Player attack hit count: " + hits.Length);

        foreach (Collider hit in hits)
        {
            WolfBossHealth wolfBossHealth = hit.GetComponentInParent<WolfBossHealth>();

            if (wolfBossHealth != null)
            {
                Debug.Log("Player hit wolf!");
                wolfBossHealth.TakeDamage(playerAttackDamage);
                break;
            }
        }
    }
}