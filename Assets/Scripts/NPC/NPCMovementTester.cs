using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public class NPCMovementTester : MonoBehaviour
{
    [SerializeField] private Vector2 testTarget = new Vector2(0, 4);

    private NPCMovement movement;

    void Awake()
    {
        movement = GetComponent<NPCMovement>();
        movement.OnTargetReached += HandleArrival;
    }

    void OnDestroy()
    {
        if (movement != null)
        {
            movement.OnTargetReached -= HandleArrival;
        }
    }

    void Start()
    {
        Debug.Log($"[TEST] Hedefe gidiyor: {testTarget}");
        movement.MoveTo(testTarget);
    }

    void HandleArrival()
    {
        Debug.Log("[TEST] Hedefe varıldı ✓");
    }
}