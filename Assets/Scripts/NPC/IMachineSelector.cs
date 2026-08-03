public interface IMachineSelector
{
    /// <summary>
    /// NPC için uygun bir makine seçer. Uygun makine yoksa null döner.
    /// </summary>
    ArcadeMachine SelectMachine(NPCController npc);
}