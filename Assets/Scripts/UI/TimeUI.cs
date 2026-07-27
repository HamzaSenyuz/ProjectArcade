using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dayText;

    void OnEnable()
    {
        GameTimeManager.OnTimeChanged += UpdateTimeDisplay;
        GameTimeManager.OnDayStarted += UpdateDayDisplay;
    }

    void OnDisable()
    {
        GameTimeManager.OnTimeChanged -= UpdateTimeDisplay;
        GameTimeManager.OnDayStarted -= UpdateDayDisplay;
    }

    void Start()
    {
        if (GameTimeManager.Instance != null)
        {
            UpdateTimeDisplay(GameTimeManager.Instance.CurrentHour, GameTimeManager.Instance.CurrentMinute);
            UpdateDayDisplay(GameTimeManager.Instance.CurrentDay);
        }
    }

    private void UpdateTimeDisplay(int hour, int minute)
    {
        if (timeText != null)
        {
            timeText.text = $"{hour:00}:{minute:00}";
        }
    }

    private void UpdateDayDisplay(int day)
    {
        if (dayText != null)
        {
            dayText.text = $"Gün {day}";
        }
    }
}