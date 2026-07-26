using UnityEngine;

public class ArcadeMachineInteraction : MonoBehaviour
{
    private bool playerInRange = false;

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
            Debug.Log("Makineye yaklaştın! E'ye bas.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Makineden uzaklaştın.");
        }
    }

    void StartMiniGame()
    {
        Debug.Log("Mini oyun başladı!");
    }
}