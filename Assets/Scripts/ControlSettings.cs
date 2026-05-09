using UnityEngine;
using UnityEngine.UI;

public class ControlSettings : MonoBehaviour
{
    [SerializeField] private Slider toggleKeySlider;

    void Start()
    {
        int savedControlScheme = PlayerPrefs.GetInt("ControlScheme", 0);

        toggleKeySlider.SetValueWithoutNotify(savedControlScheme);
        toggleKeySlider.onValueChanged.AddListener(UpdateControlScheme);
    }

    public void UpdateControlScheme(float value)
    {
        int controlScheme = Mathf.RoundToInt(value);

        PlayerPrefs.SetInt("ControlScheme", controlScheme);
        PlayerPrefs.Save();

        toggleKeySlider.SetValueWithoutNotify(controlScheme);
    }
}