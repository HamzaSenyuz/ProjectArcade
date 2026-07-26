using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMiniGame(string miniGameSceneName, string machineId, Vector2 playerPosition)
    {
        // Salondan çıkarken durumu kaydet
        GameManager.Instance.lastMachineId = machineId;
        GameManager.Instance.lastPlayerPosition = playerPosition;

        Debug.Log($"Mini oyuna geçiliyor: {miniGameSceneName}");
        SceneManager.LoadScene(miniGameSceneName);
    }

    public void ReturnToSalon()
    {
        Debug.Log("Salona dönülüyor...");
        SceneManager.LoadScene("ArcadeSalon");
    }
}