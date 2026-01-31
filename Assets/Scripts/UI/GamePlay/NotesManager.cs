using UnityEngine;
using UnityEngine.UI;

public class NotesManager : MonoBehaviour
{
    public Image PanelNotes;
    public Image PanelNotes2;

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
        PanelNotes2.gameObject.SetActive(false);
    }

    public void NextNotes()
    {
        PanelNotes2.gameObject.SetActive(true);
        PanelNotes.gameObject.SetActive(false);
    }

    public void PreviousNotes()
    {
        PanelNotes.gameObject.SetActive(true);
        PanelNotes2.gameObject.SetActive(false);
    }
}
