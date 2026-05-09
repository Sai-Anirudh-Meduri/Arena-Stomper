using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("UI")]
    [SerializeField] private Slider playerHPSlider;

    [Header("Canvas References")]
    [SerializeField] private GameObject playerHUDCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    [Header("Death Settings")]
    [SerializeField] private float deathAnimationDuration = 3f;

    [Header("Shield Settings")]
    [SerializeField] private float blockedDamageTaken = 0f;

    private Animator animator;
    private PlayerMovement playerMovement;

    private bool isDead = false;
    private ScoreTimerManager scoreTimerManager;

    void Start()
    {
        scoreTimerManager = FindFirstObjectByType<ScoreTimerManager>();
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        UpdateHealthUI();

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        if (playerMovement != null && playerMovement.IsBlocking())
        {
            damage = blockedDamageTaken;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Player Health: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (playerHPSlider != null)
        {
            playerHPSlider.value = (currentHealth / maxHealth) * 100f;
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Player died.");

        animator.SetTrigger("Die");

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Invoke(nameof(ShowGameOverScreen), deathAnimationDuration);
    }

    void ShowGameOverScreen()
    {
        if (playerHUDCanvas != null)
        {
            playerHUDCanvas.SetActive(false);
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (scoreTimerManager != null)
        {
            scoreTimerManager.ShowGameOverStats();
        }

        Time.timeScale = 0f;
    }
}