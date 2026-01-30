using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaskStateController : MonoBehaviour, IDropHandler
{
    [Header("Sprites")]
    public Sprite baseMask1;
    public Sprite baseMask2;
    public Sprite baseMask3;

    public Sprite maskWithEye;
    public Sprite maskWithMouth;
    public Sprite maskWithEyeMouth;

    private Image image;

    private bool hasBase = false;
    private bool hasEye = false;
    private bool hasMouth = false;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DragableItem item = eventData.pointerDrag.GetComponent<DragableItem>();
        if (item == null) return;

        switch (item.itemType)
        {
            case ItemType.BaseMask:
                if (!hasBase)
                {
                    image.sprite = item.baseMaskSprite;
                    hasBase = true;
                    Destroy(item.gameObject);
                }
                break;

            case ItemType.Eye:
                if (hasBase && !hasEye)
                {
                    hasEye = true;
                    UpdateMaskSprite();
                    Destroy(item.gameObject);
                }
                break;

            case ItemType.Mouth:
                if (hasBase && !hasMouth)
                {
                    hasMouth = true;
                    UpdateMaskSprite();
                    Destroy(item.gameObject);
                }
                break;
        }
    }

    void UpdateMaskSprite()
    {
        if (hasEye && hasMouth)
        {
            image.sprite = maskWithEyeMouth;
        }
        else if (hasEye)
        {
            image.sprite = maskWithEye;
        }
        else if (hasMouth)
        {
            image.sprite = maskWithMouth;
        }
    }
}
