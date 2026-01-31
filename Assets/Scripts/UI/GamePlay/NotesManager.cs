using UnityEngine;
using UnityEngine.UI;

public class NotesManager : MonoBehaviour
{
    public Image PanelNotes;

    void Start()
    {
        PanelNotes.gameObject.SetActive(false); 
    }

    public void ShowNotes()
    {
        PanelNotes.gameObject.SetActive(true);
    }

    public void HideNotes()
    {
        PanelNotes.gameObject.SetActive(false);
    }
}
