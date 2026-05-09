using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseCanvas;

    [Header("Information Canvas")]
    [SerializeField] private InformationCanvasManager informationManager;

    private bool isPaused = false;

    void Start()
    {
        pauseCanvas.SetActive(false);
    }

    void Update()
    {
        if (informationManager != null && informationManager.IsInformationOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseCanvas.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}