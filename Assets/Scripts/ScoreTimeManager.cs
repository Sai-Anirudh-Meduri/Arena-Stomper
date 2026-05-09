using UnityEngine;
using TMPro;

public class ScoreTimerManager : MonoBehaviour
{
    [Header("HUD UI")]
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private TMP_Text txtTimer;

    [Header("Game Over UI")]
    [SerializeField] private TMP_Text txtBossCount;
    [SerializeField] private TMP_Text txtTimeLasted;
    [SerializeField] private TMP_Text txtFinalScore;

    [Header("Score Settings")]
    [SerializeField] private int pointsPerSecond = 100;
    [SerializeField] private int minuteBonus = 3000;
    [SerializeField] private int wolfKillBonus = 5000;

    private float elapsedTime = 0f;
    private int score = 0;
    private int bonusScore = 0;
    private int wolvesSlain = 0;
    private bool timerRunning = true;

    private int lastMinuteReached = 0;

    void Start()
    {
        UpdateScoreUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (!timerRunning)
            return;

        elapsedTime += Time.deltaTime;

        int currentMinute = Mathf.FloorToInt(elapsedTime / 60f);

        if (currentMinute > lastMinuteReached)
        {
            bonusScore += minuteBonus;
            lastMinuteReached = currentMinute;
        }

        score = Mathf.FloorToInt(elapsedTime * pointsPerSecond) + bonusScore;

        UpdateScoreUI();
        UpdateTimerUI();
    }

    void UpdateScoreUI()
    {
        if (txtScore != null)
        {
            txtScore.text = "Score: " + score;
        }
    }

    void UpdateTimerUI()
    {
        if (txtTimer != null)
        {
            txtTimer.text = FormatTime(elapsedTime);
        }
    }

    string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void AddWolfKillScore()
    {
        wolvesSlain++;

        bonusScore += wolfKillBonus;
        score = Mathf.FloorToInt(elapsedTime * pointsPerSecond) + bonusScore;

        UpdateScoreUI();
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ShowGameOverStats()
    {
        StopTimer();

        if (txtBossCount != null)
        {
            txtBossCount.text = "Wolves Slain: " + wolvesSlain;
        }

        if (txtTimeLasted != null)
        {
            txtTimeLasted.text = "Time Lasted: " + FormatTime(elapsedTime);
        }

        if (txtFinalScore != null)
        {
            txtFinalScore.text = "Final Score: " + score;
        }
    }
}