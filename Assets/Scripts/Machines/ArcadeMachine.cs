using UnityEngine;

public class ArcadeMachine : MonoBehaviour
{
    [Header("Kimlik")]
    public string machineId = "PacMan_01";
    public MachineData data;

    [Header("Başlangıç Durumu")]
    [SerializeField] private MachineState initialState = MachineState.Active;

    private MachineState currentState;
    private bool isOccupied = false;
    private GameObject currentUser;

    public MachineState CurrentState => currentState;
    public bool IsOccupied => isOccupied;
    public bool IsAvailable => currentState == MachineState.Active && !isOccupied;

    void Start()
    {
        // GameManager'da kayıtlı durum varsa onu kullan, yoksa initialState
        if (GameManager.Instance != null)
        {
            MachineState? saved = GameManager.Instance.GetMachineState(machineId);
            currentState = saved ?? initialState;
        }
        else
        {
            currentState = initialState;
        }

        ApplyVisualState();
    }

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

    public void Release()
    {
        Debug.Log($"{data.displayName} makinesi boşaldı.");
        isOccupied = false;
        currentUser = null;
    }

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

        // Kaydet — sahne değişse bile hatırlansın
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveMachineState(machineId, currentState);
        }

        ApplyVisualState();
        Debug.Log($"{data.displayName} açıldı! (-{data.purchasePrice} jeton)");
        return true;
    }

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
            sr.color = currentState == MachineState.Active
                ? new Color(0.6f, 0.1f, 0.1f)
                : new Color(0.3f, 0.3f, 0.3f);
        }
    }
}