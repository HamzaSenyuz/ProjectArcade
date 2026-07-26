using UnityEngine;

public class ArcadeMachineInteraction : MonoBehaviour
{
    public string machineId = "PacMan";                // Bu makinenin kimliği
    public string miniGameSceneName = "MiniGame_PacMan"; // Yüklenecek sahne

    private bool playerInRange = false;
    private Transform playerTransform;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartMiniGame();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;
            Debug.Log("Makineye yaklaştın! E'ye bas.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
        }
    }

    void StartMiniGame()
    {
        Vector2 currentPos = playerTransform.position;
        SceneLoader.Instance.LoadMiniGame(miniGameSceneName, machineId, currentPos);
    }
}