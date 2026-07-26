using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform targetMachine;    // Hangi makineye gidecek
    public float playDuration = 5f;    // Kaç saniye oynayacak

    private Rigidbody2D rb;
    private float playTimer = 0f;

    private enum NPCState { Entering, Walking, Playing, Leaving }
    private NPCState currentState = NPCState.Walking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Walking:
                WalkToMachine();
                break;

            case NPCState.Playing:
                PlayAtMachine();
                break;

            case NPCState.Leaving:
                LeaveArcade();
                break;
        }
    }

    void WalkToMachine()
    {
        if (targetMachine == null) return;

        Vector2 direction = (targetMachine.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetMachine.position);

        if (distance > 0.5f)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            currentState = NPCState.Playing;
            Debug.Log("NPC makinede oynamaya başladı!");
        }
    }

    void PlayAtMachine()
    {
        playTimer += Time.deltaTime;

        if (playTimer >= playDuration)
        {
            currentState = NPCState.Leaving;
            Debug.Log("NPC ayrılıyor!");
        }
    }

    void LeaveArcade()
    {
        Vector2 exitDirection = Vector2.left;
        rb.MovePosition(rb.position + exitDirection * moveSpeed * Time.fixedDeltaTime);

        if (transform.position.x < -10f)
        {
            Debug.Log("NPC salonu terk etti.");
            Destroy(gameObject);
        }
    }
}