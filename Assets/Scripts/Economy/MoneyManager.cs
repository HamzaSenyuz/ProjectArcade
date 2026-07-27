using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] private int startingMoney = 100;
    private int currentMoney;

    // Event: para değişince kim dinliyorsa haberdar olsun
    public static event Action<int> OnMoneyChanged;

    // Dışarıdan okunabilir ama yazılamaz (encapsulation)
    public int CurrentMoney => currentMoney;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentMoney = startingMoney;
    }

    void Start()
    {
        // İlk değeri de bildir (UI güncellensin)
        OnMoneyChanged?.Invoke(currentMoney);
    }

    /// <summary>
    /// Para ekler (NPC ödeme, mini oyun ödülü vb.)
    /// </summary>
    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"AddMoney: geçersiz miktar ({amount})");
            return;
        }

        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);
        Debug.Log($"+{amount} jeton kazandın. Toplam: {currentMoney}");
    }

    /// <summary>
    /// Para harcar. Yeterli değilse false döner, para düşmez.
    /// </summary>
    public bool SpendMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"SpendMoney: geçersiz miktar ({amount})");
            return false;
        }

        if (currentMoney < amount)
        {
            Debug.Log($"Yetersiz jeton. Gerekli: {amount}, mevcut: {currentMoney}");
            return false;
        }

        currentMoney -= amount;
        OnMoneyChanged?.Invoke(currentMoney);
        Debug.Log($"-{amount} jeton harcadın. Toplam: {currentMoney}");
        return true;
    }

    /// <summary>
    /// Yeterli para var mı diye kontrol eder (satın alma butonunu aktifleştirmek için)
    /// </summary>
    public bool CanAfford(int amount)
    {
        return currentMoney >= amount;
    }
}