using UnityEngine;

public class ArcadeMachine : MonoBehaviour
{
    [Header("Kimlik")]
    public string machineId = "PacMan_01";     // Sahnedeki benzersiz ID
    public MachineData data;                    // ScriptableObject referansı

    [Header("Durum")]
    [SerializeField] private MachineState currentState = MachineState.Active;

    // Runtime durumu
    private bool isOccupied = false;
    private GameObject currentUser;    // Şu an kim oynuyor (NPC veya Player)

    // Dışa açık okuma
    public MachineState CurrentState => currentState;
    public bool IsOccupied => isOccupied;
    public bool IsAvailable => currentState == MachineState.Active && !isOccupied;

    void Start()
    {
        ApplyVisualState();
    }

    /// <summary>
    /// Makineyi kullanmaya başlar (NPC veya oyuncu tarafından çağrılır)
    /// </summary>
    public bool TryOccupy(GameObject user)
    {
        if (!IsAvailable)
        {
            Debug.Log($"{data.displayName} şu an müsait değil.");
            return false;
        }

        isOccupied = true;
        currentUser = user;
        Debug.Log($"{user.name}, {data.displayName} makinesini kullanmaya başladı.");
        return true;
    }

    /// <summary>
    /// Makineyi serbest bırakır
    /// </summary>
    public void Release()
    {
        Debug.Log($"{data.displayName} makinesi boşaldı.");
        isOccupied = false;
        currentUser = null;
    }

    /// <summary>
    /// Kilidini aç (ödeme yapıldıktan sonra)
    /// </summary>
    public bool TryUnlock()
    {
        if (currentState != MachineState.Purchasable)
        {
            Debug.Log($"{data.displayName} satın alınabilir durumda değil.");
            return false;
        }

        if (!MoneyManager.Instance.SpendMoney(data.purchasePrice))
        {
            Debug.Log("Yetersiz jeton, makine açılamadı.");
            return false;
        }

        currentState = MachineState.Active;
        ApplyVisualState();
        Debug.Log($"{data.displayName} açıldı! (-{data.purchasePrice} jeton)");
        return true;
    }

    /// <summary>
    /// Görsel durumu günceller (kilitli / aktif)
    /// </summary>
    private void ApplyVisualState()
    {
        if (data == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        if (currentState == MachineState.Active && data.activeSprite != null)
        {
            sr.sprite = data.activeSprite;
            sr.color = Color.white;
        }
        else if (data.lockedSprite != null)
        {
            sr.sprite = data.lockedSprite;
        }
        else
        {
            // Sprite yoksa sadece rengi değiştir (şu anki placeholder senaryosu)
            sr.color = currentState == MachineState.Active
                ? new Color(0.6f, 0.1f, 0.1f)  // koyu kırmızı = aktif
                : new Color(0.3f, 0.3f, 0.3f); // gri = kilitli
        }
    }
}