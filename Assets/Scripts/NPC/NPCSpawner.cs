using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 8f;   // Her X saniyede bir spawn dene
    [SerializeField] private int maxNPCsInSalon = 5;      // Salonda aynı anda en fazla kaç NPC

    private float spawnTimer;
    private bool isSpawningActive = false;

    void OnEnable()
    {
        GameTimeManager.OnSalonOpened += HandleSalonOpened;
        GameTimeManager.OnSalonClosed += HandleSalonClosed;
    }

    void OnDisable()
    {
        GameTimeManager.OnSalonOpened -= HandleSalonOpened;
        GameTimeManager.OnSalonClosed -= HandleSalonClosed;
    }

    void Start()
    {
        // Sahne yüklendiğinde salon zaten açıksa spawn'ı başlat
        if (GameTimeManager.Instance != null && GameTimeManager.Instance.IsSalonOpen)
        {
            isSpawningActive = true;
            spawnTimer = 0f;
        }
    }

    void Update()
    {
        if (!isSpawningActive) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnNPC();
        }
    }

    private void HandleSalonOpened()
    {
        Debug.Log("[Spawner] Salon açıldı, NPC spawn başlıyor.");
        isSpawningActive = true;
        spawnTimer = 0f;
    }

    private void HandleSalonClosed()
    {
        Debug.Log("[Spawner] Salon kapandı, NPC spawn durdu.");
        isSpawningActive = false;
    }

    private void TrySpawnNPC()
    {
        if (npcPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("[Spawner] Prefab veya spawn noktası atanmamış.");
            return;
        }

        // Sahnedeki mevcut NPC sayısını kontrol et
        int currentCount = GetActiveNPCCount();
        if (currentCount >= maxNPCsInSalon)
        {
            // Salon dolu, spawn yapma
            return;
        }

        Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"[Spawner] NPC spawn edildi. Salondaki toplam: {currentCount + 1}");
    }

    private int GetActiveNPCCount()
    {
        return Object.FindObjectsByType<NPCController>(FindObjectsSortMode.None).Length;
    }
}