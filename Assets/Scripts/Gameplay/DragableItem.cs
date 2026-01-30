using UnityEngine;
using UnityEngine.EventSystems;

public class DragableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemType itemType;
    public Sprite sprite;

    [Header("Apply to Mask")]
    public Vector2 targetSize = new Vector2(200, 200);
    public Vector2 targetPosition = Vector2.zero;
    public Vector3 targetRotation = Vector3.zero;

    RectTransform rect;
    Canvas canvas;
    CanvasGroup group;

    Vector2 startPosition;
    Vector2 startSize;
    Quaternion startRotation;
    Transform startParent;

    [HideInInspector]
    public bool isDroppedOnSlot = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        group = GetComponent<CanvasGroup>();

        
        startParent = rect.parent;
        startPosition = rect.anchoredPosition;
        startSize = rect.sizeDelta;
        startRotation = rect.localRotation;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDroppedOnSlot = false;

        group.alpha = 0.6f;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        group.alpha = 1f;
        group.blocksRaycasts = true;
        group.interactable = true;

        if (!isDroppedOnSlot)
        {
            ResetToStart();
        }
    }

    
    public void ResetToStart()
    {
        rect.SetParent(startParent);
        rect.anchoredPosition = startPosition;
        rect.sizeDelta = startSize;
        rect.localRotation = startRotation;

        isDroppedOnSlot = false;

        group.alpha = 1f;
        group.blocksRaycasts = true;
        group.interactable = true;
    }
}
