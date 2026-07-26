using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.lastPlayerPosition != Vector2.zero)
        {
            transform.position = GameManager.Instance.lastPlayerPosition;
            Debug.Log("Oyuncu son konumuna spawn edildi.");
        }
    }
}