using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "NPC/Data")]
public class NPCData : ScriptableObject
{
    public string npcId;
    public string npcName;
    public MaskNeeded requiredMask;
    public Sprite npcSprite;
}
