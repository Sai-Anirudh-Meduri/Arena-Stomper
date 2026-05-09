using UnityEngine;
using TMPro;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown audioDropdown;

    void Start()
    {
        int savedVolumeIndex = PlayerPrefs.GetInt("AudioVolume", 4);

        audioDropdown.SetValueWithoutNotify(savedVolumeIndex);

        ApplyVolume(savedVolumeIndex);

        audioDropdown.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(int index)
    {
        PlayerPrefs.SetInt("AudioVolume", index);
        PlayerPrefs.Save();

        ApplyVolume(index);
    }

    void ApplyVolume(int index)
    {
        float volume = 1f;

        switch (index)
        {
            case 0:
                volume = 0f;
                break;

            case 1:
                volume = 0.25f;
                break;

            case 2:
                volume = 0.5f;
                break;

            case 3:
                volume = 0.75f;
                break;

            case 4:
                volume = 1f;
                break;
        }

        AudioListener.volume = volume;
    }
}