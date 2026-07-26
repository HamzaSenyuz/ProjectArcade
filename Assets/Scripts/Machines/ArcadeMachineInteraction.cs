using UnityEngine;

public class ArcadeMachineInteraction : MonoBehaviour
{
    public string machineId = "PacMan";
    public string miniGameSceneName = "MiniGame_PacMan";

    [Header("UI")]
    public GameObject interactionPromptPrefab;  // Inspector'dan atanacak
    public Vector3 promptOffset = new Vector3(0, 1.5f, 0);  // Makinenin üstünde nerede duracak

    private GameObject currentPrompt;  // Şu an gösterilen prompt'un referansı
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

    void ShowPrompt()
    {
        if (interactionPromptPrefab == null) return;
        if (currentPrompt != null) return;  // Zaten gösteriliyor

        currentPrompt = Instantiate(
            interactionPromptPrefab,
            transform.position + promptOffset,
            Quaternion.identity,
            transform  // Makinenin çocuğu olsun
        );
    }

    void HidePrompt()
    {
        if (currentPrompt != null)
        {
            Destroy(currentPrompt);
            currentPrompt = null;
        }
    }

    void StartMiniGame()
    {
        HidePrompt();  // Sahne değişmeden önce temizle
        Vector2 currentPos = playerTransform.position;
        SceneLoader.Instance.LoadMiniGame(miniGameSceneName, machineId, currentPos);
    }
}