using UnityEngine;

public class MoneyDebugTester : MonoBehaviour
{
    void OnEnable()
    {
        // Event'e abone ol
        MoneyManager.OnMoneyChanged += HandleMoneyChanged;
    }

    void OnDisable()
    {
        // Aboneliği iptal et (memory leak önlemi!)
        MoneyManager.OnMoneyChanged -= HandleMoneyChanged;
    }

    void Update()
    {
        // Sadece test için
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MoneyManager.Instance.AddMoney(50);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MoneyManager.Instance.SpendMoney(30);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MoneyManager.Instance.SpendMoney(9999);  // Yetersiz para testi
        }
    }

    void HandleMoneyChanged(int newAmount)
    {
        Debug.Log($"[DEBUG] Para dinleyici tarafından yakalandı: {newAmount}");
    }
}