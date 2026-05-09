using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WolfBossHealth : MonoBehaviour
{
    [Header("Wolf Health")]
    [SerializeField] private float maxHealth = 150f;
    [SerializeField] private float currentHealth;

    [Header("UI")]
    private Slider enemyHPSlider;

    [Header("Respawn Settings")]
    [SerializeField] private WolfBossSpawner spawner;
    [SerializeField] private float deathAnimationDuration = 3f;
    [SerializeField] private bool destroyAfterDeath = true;

    private Animator animator;
    private NavMeshAgent agent;
    private WolfBossAI wolfAI;
    private ScoreTimerManager scoreManager;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        wolfAI = GetComponent<WolfBossAI>();

        scoreManager = FindFirstObjectByType<ScoreTimerManager>();

        if (spawner == null)
        {
            spawner = FindFirstObjectByType<WolfBossSpawner>();
        }

        enemyHPSlider = GameObject.Find("EnemyHPSlider").GetComponent<Slider>();

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Wolf Health: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("GotHit");
        }
    }

    void UpdateHealthUI()
    {
        if (enemyHPSlider != null)
        {
            enemyHPSlider.value = (currentHealth / maxHealth) * 100f;
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Wolf died.");

        if (scoreManager != null)
        {
            scoreManager.AddWolfKillScore();
        }

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (wolfAI != null)
        {
            wolfAI.enabled = false;
        }

        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetTrigger("Die");

        Invoke(nameof(SpawnNextWolf), deathAnimationDuration);
    }

    void SpawnNextWolf()
    {
        if (spawner != null)
        {
            spawner.SpawnWolf();
        }

        if (destroyAfterDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            enabled = false;
        }
    }
}