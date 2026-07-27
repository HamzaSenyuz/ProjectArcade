using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Kalıcı veriler
    public Vector2 lastPlayerPosition;
    public string lastMachineId;

    // Makine durumları: machineId → MachineState
    public Dictionary<string, MachineState> machineStates = new Dictionary<string, MachineState>();

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

    /// <summary>
    /// Bir makinenin durumunu kaydeder (satın alındığında vs.)
    /// </summary>
    public void SaveMachineState(string machineId, MachineState state)
    {
        machineStates[machineId] = state;
    }

    /// <summary>
    /// Makinenin kaydedilmiş durumunu döner. Yoksa null döner.
    /// </summary>
    public MachineState? GetMachineState(string machineId)
    {
        if (machineStates.TryGetValue(machineId, out MachineState state))
        {
            return state;
        }
        return null;
    }
}