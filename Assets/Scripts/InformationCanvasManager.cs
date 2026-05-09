using UnityEngine;

public class InformationCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject informationCanvas;

    public bool IsInformationOpen { get; private set; }

    void Start()
    {
        IsInformationOpen = true;
        informationCanvas.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseInformationCanvas()
    {
        IsInformationOpen = false;
        informationCanvas.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}