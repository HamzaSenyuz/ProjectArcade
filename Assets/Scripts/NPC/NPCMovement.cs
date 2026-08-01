using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float arrivalThreshold = 0.15f;

    private Rigidbody2D rb;
    private Vector2? currentTarget;   // null = hedef yok, dur

    // Event: hedefe varıldığında haber ver
    public event Action OnTargetReached;

    public bool HasTarget => currentTarget.HasValue;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!currentTarget.HasValue)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 target = currentTarget.Value;
        Vector2 toTarget = target - rb.position;
        float distance = toTarget.magnitude;

        if (distance <= arrivalThreshold)
        {
            rb.linearVelocity = Vector2.zero;
            currentTarget = null;
            OnTargetReached?.Invoke();
            return;
        }

        Vector2 direction = toTarget / distance; // normalized
        rb.linearVelocity = direction * moveSpeed;
    }

    /// <summary>
    /// Yeni hedef ata. NPC oraya doğru hareket etmeye başlar.
    /// </summary>
    public void MoveTo(Vector2 target)
    {
        currentTarget = target;
    }

    /// <summary>
    /// Hareketi durdur.
    /// </summary>
    public void Stop()
    {
        currentTarget = null;
        rb.linearVelocity = Vector2.zero;
    }
}