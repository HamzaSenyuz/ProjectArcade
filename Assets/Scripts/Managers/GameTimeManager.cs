using System;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;

    [Header("Zaman Ayarları")]
    [SerializeField] private float realSecondsPerDay = 600f;  // 1 oyun günü = 10 dakika
    [SerializeField] private int openingHour = 8;             // 08:00
    [SerializeField] private int closingHour = 22;            // 22:00

    // Oyun içi zaman (dakika olarak tutuyoruz)
    private float currentMinutes;    // 0..1440 (bir günde 1440 dakika)
    private int currentDay = 1;
    private bool isSalonOpen = false;
    private bool isDayEnded = false;  // Gün bitti, yeni gün için bekleniyor
    public bool IsDayEnded => isDayEnded;

    // Events
    public static event Action<int, int> OnTimeChanged;   // (saat, dakika)
    public static event Action<int> OnHourChanged;         // saat
    public static event Action<int> OnDayStarted;          // gün no
    public static event Action<int> OnDayEnded;            // gün no
    public static event Action OnSalonOpened;
    public static event Action OnSalonClosed;

    // Dışa açık okuma
    public int CurrentHour => (int)(currentMinutes / 60);
    public int CurrentMinute => (int)(currentMinutes % 60);
    public int CurrentDay => currentDay;
    public bool IsSalonOpen => isSalonOpen;

    private int lastAnnouncedHour = -1;

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
            return;
        }

        // Günü açılış saatinde başlat
        currentMinutes = openingHour * 60;
    }

    void Start()
    {
        StartNewDay();
    }

    void Update()
    {
        AdvanceTime();

        // === DEBUG (sonra sil) ===
        if (Input.GetKeyDown(KeyCode.N))  // N = Next Day
        {
            StartNextDay();
        }
        // ==========================
    }

    private void AdvanceTime()
    {
        // Gün bittiyse zaman durur, oyuncu yeni günü başlatana kadar bekle
        if (isDayEnded) return;

        float minutesPerSecond = 1440f / realSecondsPerDay;
        currentMinutes += minutesPerSecond * Time.deltaTime;

        OnTimeChanged?.Invoke(CurrentHour, CurrentMinute);

        if (CurrentHour != lastAnnouncedHour)
        {
            lastAnnouncedHour = CurrentHour;
            OnHourChanged?.Invoke(CurrentHour);
            CheckSalonSchedule();
        }
    }

    private void CheckSalonSchedule()
    {
        // Kapanış saatine ulaşıldı mı?
        if (CurrentHour >= closingHour && isSalonOpen)
        {
            CloseSalon();
        }
    }

    private void StartNewDay()
    {
        isSalonOpen = false;
        lastAnnouncedHour = -1;
        Debug.Log($"═══ Gün {currentDay} başladı ═══");
        OnDayStarted?.Invoke(currentDay);
        OpenSalon();
    }

    private void OpenSalon()
    {
        isSalonOpen = true;
        Debug.Log($"🔓 Salon açıldı ({openingHour:00}:00)");
        OnSalonOpened?.Invoke();
    }

    private void CloseSalon()
    {
        isSalonOpen = false;
        isDayEnded = true;    // Gün bitti bayrağını kaldır
        Debug.Log($"🔒 Salon kapandı ({closingHour:00}:00)");
        OnSalonClosed?.Invoke();
        OnDayEnded?.Invoke(currentDay);
        Debug.Log("⏸️ Yeni gün başlaması için 'StartNextDay()' bekleniyor.");
    }

    /// <summary>
    /// Zamanı manuel ayarlamak için (test/debug)
    /// </summary>
    public void SetTime(int hour, int minute)
    {
        currentMinutes = hour * 60 + minute;
        lastAnnouncedHour = -1;
    }
    /// <summary>
    /// Yeni günü manuel olarak başlatır (oyuncu butona bastığında çağrılır)
    /// </summary>
    public void StartNextDay()
    {
        if (!isDayEnded)
        {
            Debug.LogWarning("Gün henüz bitmedi, yeni gün başlatılamaz.");
            return;
        }

        isDayEnded = false;
        currentDay++;
        currentMinutes = openingHour * 60;
        StartNewDay();
    }
}