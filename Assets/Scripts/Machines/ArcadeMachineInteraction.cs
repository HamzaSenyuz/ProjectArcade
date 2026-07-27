using UnityEngine;

[RequireComponent(typeof(ArcadeMachine))]
public class ArcadeMachineInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactionPromptPrefab;
    public Vector3 promptOffset = new Vector3(0, -1.5f, 0);

    private ArcadeMachine machine;
    private GameObject currentPrompt;
    private bool playerInRange = false;
    private Transform playerTransform;

    void Awake()
    {
        machine = GetComponent<ArcadeMachine>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;
            ShowPrompt();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
            HidePrompt();
        }
    }

    void HandleInteraction()
    {
        switch (machine.CurrentState)
        {
            case MachineState.Active:
                StartMiniGame();
                break;

            case MachineState.Purchasable:
                machine.TryUnlock();
                UpdatePromptText();
                break;

            case MachineState.Locked:
                Debug.Log("Bu makine şu an alınamaz.");
                break;
        }
    }

    void StartMiniGame()
    {
        if (machine.IsOccupied)
        {
            Debug.Log("Makine dolu, biraz bekle.");
            return;
        }

        HidePrompt();
        Vector2 currentPos = playerTransform.position;
        SceneLoader.Instance.LoadMiniGame(
            machine.data.miniGameSceneName,
            machine.machineId,
            currentPos
        );
    }

    void ShowPrompt()
    {
        if (interactionPromptPrefab == null) return;
        if (currentPrompt != null) return;

        currentPrompt = Instantiate(
            interactionPromptPrefab,
            transform.position + promptOffset,
            Quaternion.identity,
            transform
        );
        UpdatePromptText();
    }

    void UpdatePromptText()
    {
        if (currentPrompt == null) return;

        var text = currentPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (text == null) return;

        switch (machine.CurrentState)
        {
            case MachineState.Active:
                text.text = "[E] Oyna";
                break;
            case MachineState.Purchasable:
                text.text = $"[E] Satın Al ({machine.data.purchasePrice})";
                break;
            case MachineState.Locked:
                text.text = "Kilitli";
                break;
        }
    }

    void HidePrompt()
    {
        if (currentPrompt != null)
        {
            Destroy(currentPrompt);
            currentPrompt = null;
        }
    }
}