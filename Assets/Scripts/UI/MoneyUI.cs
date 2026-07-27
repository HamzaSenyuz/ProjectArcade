using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private string prefix = "💰 ";

    void OnEnable()
    {
        MoneyManager.OnMoneyChanged += UpdateMoneyDisplay;
    }

    void OnDisable()
    {
        MoneyManager.OnMoneyChanged -= UpdateMoneyDisplay;
    }

    void Start()
    {
        if (MoneyManager.Instance != null)
        {
            UpdateMoneyDisplay(MoneyManager.Instance.CurrentMoney);
        }
    }

    private void UpdateMoneyDisplay(int newAmount)
    {
        if (moneyText != null)
        {
            moneyText.text = prefix + newAmount.ToString();
        }
    }
}