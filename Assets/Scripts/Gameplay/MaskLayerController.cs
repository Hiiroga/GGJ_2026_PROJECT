using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class MaskLayerController : MonoBehaviour, IDropHandler
{
    public static MaskLayerController Instance { get; private set; }
    [Header("Image Layer")]
    public Image baseLayer;
    public Image eyeLayer;
    public Image mouthLayer;

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip[] dropClips;

    private DragableItem usedBaseItem;
    private DragableItem usedEyeItem;
    private DragableItem usedMouthItem;

    void Awake()
    {
        Instance = this;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DragableItem item = eventData.pointerDrag.GetComponent<DragableItem>();
        if (item == null) return;

        switch (item.itemType)
        {
            case ItemType.BaseMask:
                if (baseLayer.sprite == null)
                {
                    ApplyToLayer(baseLayer, item);
                    usedBaseItem = item;               
                    item.isDroppedOnSlot = true;
                    item.gameObject.SetActive(false);

                    PlayRandomDropSFX();
                }
                break;

            case ItemType.Eye:
                if (eyeLayer.sprite == null)
                {
                    ApplyToLayer(eyeLayer, item);
                    usedEyeItem = item;            
                    item.isDroppedOnSlot = true;
                    item.gameObject.SetActive(false);

                    PlayRandomDropSFX();
                }
                break;

            case ItemType.Mouth:
                if (mouthLayer.sprite == null)
                {
                    ApplyToLayer(mouthLayer, item);
                    usedMouthItem = item;           
                    item.isDroppedOnSlot = true;
                    item.gameObject.SetActive(false);

                    PlayRandomDropSFX();
                }
                break;
        }
    }

    void PlayRandomDropSFX()
    {
        if (dropClips == null || dropClips.Length == 0) return;

        int index = Random.Range(0, dropClips.Length);
        audioSource.PlayOneShot(dropClips[index]);
    }

    public void resetLayer()
    { 
        baseLayer.sprite = null;
        eyeLayer.sprite = null;
        mouthLayer.sprite = null;

        ResetRect(baseLayer.rectTransform);
        ResetRect(eyeLayer.rectTransform);
        ResetRect(mouthLayer.rectTransform);

        if (usedBaseItem != null)
        {
            usedBaseItem.gameObject.SetActive(true);
            usedBaseItem.ResetToStart();  
            usedBaseItem = null;
        }

        if (usedEyeItem != null)
        {
            usedEyeItem.gameObject.SetActive(true);
            usedEyeItem.ResetToStart();
            usedEyeItem = null;
        }

        if (usedMouthItem != null)
        {
            usedMouthItem.gameObject.SetActive(true);
            usedMouthItem.ResetToStart();
            usedMouthItem = null;
        }
    }


    void ResetRect(RectTransform rt)
    {
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        rt.localRotation = Quaternion.identity;
    }


    void ApplyToLayer(Image layer, DragableItem item)
    {
        layer.sprite = item.sprite;

        RectTransform rt = layer.rectTransform;
        rt.sizeDelta = item.targetSize;
        rt.anchoredPosition = item.targetPosition;
        rt.localRotation = Quaternion.Euler(item.targetRotation);
    }

}
