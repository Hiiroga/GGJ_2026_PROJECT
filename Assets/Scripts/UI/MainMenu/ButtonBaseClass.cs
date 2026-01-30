using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ButtonBaseClass: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Color originalColor;
    [SerializeField] private float alphaColor = 100f;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {        
        if (buttonImage != null)
        {
            Color hoverColor = originalColor;
            hoverColor.a = alphaColor / 255f;
            buttonImage.color = hoverColor;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }

    public abstract void OnClick();
}