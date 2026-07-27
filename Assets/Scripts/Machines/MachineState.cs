public enum MachineState
{
    Locked,        // Şu an için satın alınamaz
    Purchasable,   // Kilitli ama satın alınabilir
    Active,        // Çalışıyor, kullanılabilir
    Broken         // İleride: bozuk
}