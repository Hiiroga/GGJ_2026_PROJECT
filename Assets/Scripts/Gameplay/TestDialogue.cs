using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    public void OnClick()
    {
        NPCQueueManager.Instance.ServeCurrentNPC(MaskNeeded.Happy);
    }
}