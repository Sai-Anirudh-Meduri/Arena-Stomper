using UnityEngine;
using UnityEngine.AI;

public class WolfBossAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Distance Settings")]
    public float walkRange = 6f;
    public float attackRange = 3f;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 8f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;

    [Header("Hit Settings")]
    public float hitStunTime = 1f;

    private NavMeshAgent agent;
    private Animator animator;

    private float attackTimer;
    private bool isDead = false;
    private bool isStunned = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = attackRange;

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }
    }

    void Update()
    {
        if (player == null || isDead || isStunned)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Attack when very close
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        // Walk when somewhat close
        else if (distanceToPlayer <= walkRange)
        {
            WalkToPlayer();
        }
        // Run when far away
        else
        {
            RunToPlayer();
        }

        attackTimer -= Time.deltaTime;

        // Test got hit animation with G. Delete after testing.
        if (Input.GetKeyDown(KeyCode.G))
        {
            GotHit();
        }

        // Test death animation with H. Delete after testing.
        if (Input.GetKeyDown(KeyCode.H))
        {
            Die();
        }
    }

    void WalkToPlayer()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        agent.SetDestination(player.position);

        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
    }

    void RunToPlayer()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);

        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;

        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);

        FacePlayer();

        if (attackTimer <= 0f)
        {
            int randomAttack = Random.Range(1, 3);

            if (randomAttack == 1)
            {
                animator.SetTrigger("Attack1");
            }
            else
            {
                animator.SetTrigger("Attack2");
            }

            attackTimer = attackCooldown;
        }
    }

    void GotHit()
    {
        if (isDead)
            return;

        isStunned = true;
        agent.isStopped = true;

        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);

        animator.SetTrigger("GotHit");

        Invoke(nameof(EndStun), hitStunTime);
    }

    void EndStun()
    {
        isStunned = false;
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        agent.isStopped = true;

        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);

        animator.SetTrigger("Die");
    }

    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}