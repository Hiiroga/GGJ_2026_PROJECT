using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonBaseClass: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {        
        SfxManager.Instance.Play("buttonhover");        
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        
    }

    public abstract void OnClick();
}