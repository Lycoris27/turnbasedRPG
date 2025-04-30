using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro

public class SettingsController : MonoBehaviour
{
    [System.Serializable]
    public class SliderData
    {
        public string settingName;          // The key to use in PlayerPrefs
        public Slider slider;               // Direct reference to the slider
        public TextMeshProUGUI valueText;   // Direct reference to the number text
    }

    [System.Serializable]
    public class ToggleData
    {
        public string settingName;          // The key to use in PlayerPrefs
        public Toggle toggle;               // Direct reference to the toggle
    }

    [Header("Sliders")]
    [SerializeField] private List<SliderData> sliders = new List<SliderData>();

    [Header("Toggles")]
    [SerializeField] private List<ToggleData> toggles = new List<ToggleData>();

    public static event Action OnSettingsSaved;

    private void Start()
    {
        // Setup sliders
        foreach (var s in sliders)
        {
            if (s.slider != null && s.valueText != null)
            {
                // Load saved value if available
                float savedValue = PlayerPrefs.GetFloat(s.settingName, s.slider.value);
                s.slider.value = savedValue;
                s.valueText.text = Mathf.RoundToInt(savedValue * 100).ToString();

                // Add listener for live updates
                s.slider.onValueChanged.AddListener((val) =>
                {
                    s.valueText.text = Mathf.RoundToInt(val * 100).ToString();
                });
            }
        }

        // Setup toggles
        foreach (var t in toggles)
        {
            if (t.toggle != null)
            {
                // Load saved state (1 for true, 0 for false)
                bool savedState = PlayerPrefs.GetInt(t.settingName, t.toggle.isOn ? 1 : 0) == 1;
                t.toggle.isOn = savedState;

                t.toggle.onValueChanged.AddListener((isOn) =>
                {
                    // No "On/Off" text to update, so no code here
                });
            }
        }
    }

    public void SaveData()
    {
        // Save slider values
        foreach (var s in sliders)
        {
            if (s.slider != null)
            {
                PlayerPrefs.SetFloat(s.settingName, s.slider.value);
            }
        }

        // Save toggle values
        foreach (var t in toggles)
        {
            if (t.toggle != null)
            {
                // Save toggle state as 1 for On and 0 for Off
                PlayerPrefs.SetInt(t.settingName, t.toggle.isOn ? 1 : 0);
            }
        }

        PlayerPrefs.Save();
        OnSettingsSaved?.Invoke();
        Debug.Log("Settings saved.");
    }
}
