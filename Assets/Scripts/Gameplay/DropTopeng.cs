using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropTopeng : MonoBehaviour, IDropHandler
{
    public Sprite topengMulutSprite;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DragDrop draggedItem = eventData.pointerDrag.GetComponent<DragDrop>();
        if (draggedItem != null)
        {
            GetComponent<Image>().sprite = topengMulutSprite;

            // optional: hapus mulut setelah ditempel
            Destroy(eventData.pointerDrag);
        }
    }
}
