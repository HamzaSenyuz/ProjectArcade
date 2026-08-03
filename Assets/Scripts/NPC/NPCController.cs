using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public class NPCController : MonoBehaviour
{
    [Header("Davranış Ayarları")]
    [SerializeField] private float maxWaitTime = 5f;        // Boş makine yoksa ne kadar bekler
    [SerializeField] private float retryInterval = 1f;      // Bekleme sırasında kaç saniyede bir tekrar dener
    [SerializeField] private Vector2 exitPoint = new Vector2(0, -8f);  // Salondan çıkış noktası

    private NPCMovement movement;
    private IMachineSelector machineSelector;
    private ArcadeMachine targetMachine;
    private NPCState currentState;
    private float waitTimer;

    public NPCState CurrentState => currentState;

    void Awake()
    {
        movement = GetComponent<NPCMovement>();

        // Selector'ı burada oluşturuyoruz. İleride NPCSpawner'dan enjekte edebiliriz.
        machineSelector = new SimpleMachineSelector();

        movement.OnTargetReached += HandleTargetReached;
    }

    void OnDestroy()
    {
        if (movement != null)
        {
            movement.OnTargetReached -= HandleTargetReached;
        }
    }

    void Start()
    {
        TransitionTo(NPCState.SearchingForMachine);
    }

    void Update()
    {
        // Sadece bazı state'ler update mantığı gerektirir
        if (currentState == NPCState.WaitingForFreeMachine)
        {
            UpdateWaiting();
        }
    }

    // === STATE MANAGEMENT ===

    private void TransitionTo(NPCState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case NPCState.SearchingForMachine:
                SearchForMachine();
                break;

            case NPCState.WalkingToMachine:
                // WalkingToMachine, SearchForMachine içinde ateşleniyor
                break;

            case NPCState.WaitingForFreeMachine:
                waitTimer = 0f;
                movement.Stop();
                Debug.Log($"{name}: Boş makine yok, bekliyorum...");
                break;

            case NPCState.Playing:
                // Playing state, HandleTargetReached içinde başlıyor
                break;

            case NPCState.Leaving:
                Debug.Log($"{name}: Salondan ayrılıyorum.");
                movement.MoveTo(exitPoint);
                break;
        }
    }

    // === STATE BEHAVIORS ===

    private void SearchForMachine()
    {
        ArcadeMachine chosen = machineSelector.SelectMachine(this);

        if (chosen == null)
        {
            TransitionTo(NPCState.WaitingForFreeMachine);
            return;
        }

        targetMachine = chosen;
        Debug.Log($"{name}: {chosen.data.displayName} seçildi, yürüyorum.");
        movement.MoveTo(chosen.transform.position);
        TransitionTo(NPCState.WalkingToMachine);
    }

    private void UpdateWaiting()
    {
        waitTimer += Time.deltaTime;

        // Belirli aralıklarla tekrar dene
        if (waitTimer % retryInterval < Time.deltaTime)
        {
            ArcadeMachine chosen = machineSelector.SelectMachine(this);
            if (chosen != null)
            {
                targetMachine = chosen;
                Debug.Log($"{name}: {chosen.data.displayName} boşaldı, yürüyorum.");
                movement.MoveTo(chosen.transform.position);
                TransitionTo(NPCState.WalkingToMachine);
                return;
            }
        }

        // Süre dolduysa çık
        if (waitTimer >= maxWaitTime)
        {
            Debug.Log($"{name}: Çok bekledim, ayrılıyorum.");
            TransitionTo(NPCState.Leaving);
        }
    }

    private void HandleTargetReached()
    {
        switch (currentState)
        {
            case NPCState.WalkingToMachine:
                TryStartPlaying();
                break;

            case NPCState.Leaving:
                Debug.Log($"{name}: Salondan çıktım.");
                Destroy(gameObject);
                break;
        }
    }

    private void TryStartPlaying()
    {
        if (targetMachine == null)
        {
            TransitionTo(NPCState.SearchingForMachine);
            return;
        }

        // Yürürken makine dolmuş olabilir!
        if (!targetMachine.TryOccupy(gameObject))
        {
            targetMachine = null;
            TransitionTo(NPCState.SearchingForMachine);
            return;
        }

        // Ödemeyi yap
        MoneyManager.Instance.AddMoney(targetMachine.data.incomePerPlay);

        // Oyna
        currentState = NPCState.Playing;
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        float duration = targetMachine.data.playDuration;
        Debug.Log($"{name}: {duration} saniye oynayacağım.");
        yield return new WaitForSeconds(duration);

        targetMachine.Release();
        targetMachine = null;

        TransitionTo(NPCState.Leaving);
    }
}