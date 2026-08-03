using System.Collections.Generic;
using UnityEngine;

public class SimpleMachineSelector : IMachineSelector
{
    public ArcadeMachine SelectMachine(NPCController npc)
    {
        // Sahnedeki tüm ArcadeMachine'leri bul
        ArcadeMachine[] allMachines = Object.FindObjectsByType<ArcadeMachine>(FindObjectsSortMode.None);

        // Sadece uygun (Active + boş) olanları filtrele
        List<ArcadeMachine> available = new List<ArcadeMachine>();
        foreach (var machine in allMachines)
        {
            if (machine.IsAvailable)
            {
                available.Add(machine);
            }
        }

        if (available.Count == 0)
        {
            return null;  // Boş makine yok
        }

        // Rastgele birini seç
        int index = Random.Range(0, available.Count);
        return available[index];
    }
}