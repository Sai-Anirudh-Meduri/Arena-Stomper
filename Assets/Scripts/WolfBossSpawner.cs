using UnityEngine;
using TMPro;

public class WolfBossSpawner : MonoBehaviour
{
    [Header("Wolf Prefab")]
    [SerializeField] private GameObject wolfBossPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;

    [Header("UI")]
    [SerializeField] private TMP_Text txtEnemy;

    private int wolfCount = 1;

    void Start()
    {
        UpdateEnemyText();
    }

    public void SpawnWolf()
    {
        if (wolfBossPrefab != null && spawnPoint != null)
        {
            wolfCount++;

            Instantiate(
                wolfBossPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            UpdateEnemyText();
        }
    }

    void UpdateEnemyText()
    {
        if (txtEnemy != null)
        {
            txtEnemy.text = "Wolf#" + wolfCount;
        }
    }
}