using UnityEngine;
using UnityEngine.EventSystems;

public class ClickBrush : MonoBehaviour, IPointerClickHandler
{
    public NotesManager notesManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        notesManager.ShowNotes();
    }
}
