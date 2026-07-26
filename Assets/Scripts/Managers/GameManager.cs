using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton referansı

    // Kalıcı veriler
    public int playerMoney = 0;
    public Vector2 lastPlayerPosition;    // Salondaki son konum
    public string lastMachineId;          // Hangi makineye girdi

    void Awake()
    {
        // Singleton kontrolü
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Sahne değişince silme!
        }
        else
        {
            Destroy(gameObject);  // Zaten bir tane var, bunu sil
        }
    }
}