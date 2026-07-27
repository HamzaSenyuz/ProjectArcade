using UnityEngine;

[CreateAssetMenu(fileName = "New Machine Data", menuName = "ProjectArcade/Machine Data")]
public class MachineData : ScriptableObject
{
    [Header("Kimlik")]
    public string displayName = "Yeni Makine";
    public string miniGameSceneName = "MiniGame_PacMan";

    [Header("Ekonomi")]
    public int purchasePrice = 500;    // Satın alma fiyatı
    public int incomePerPlay = 10;     // NPC oynayınca kazanılan jeton

    [Header("Zamanlama")]
    public float playDuration = 15f;   // NPC ne kadar oynayacak (saniye)

    [Header("Görsel (opsiyonel)")]
    public Sprite activeSprite;        // Aktif haldeki görsel
    public Sprite lockedSprite;        // Kilitli/tozlu görsel
}