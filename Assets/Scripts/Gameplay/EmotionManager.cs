using UnityEngine;
using UnityEngine.UI;

public class EmotionManager : MonoBehaviour
{
    [Header("Current Emotion")]
    public MaskNeeded selectedEmotion;

    [Header("Emotion Configs")]
    public EmotionConfig[] emotionConfigs;

    [Header("Mask Sprites")]
    public Sprite maskPutih;
    public Sprite maskHitam;

    [Header("Eye Sprites")]
    public Sprite eyeNormal;
    public Sprite eyeRetak;

    [Header("Mouth Sprites")]
    public Sprite mouthSenyum;
    public Sprite mouthCemberut;
    public Sprite mouthFlat;

    [Header("Layers")]
    public Image baseLayer;
    public Image eyeLayer;
    public Image mouthLayer;

    [Header("UI Button")]
    public Button ApplyButton;


    public void CheckCurrentEmotion()
    {
        if (baseLayer.sprite == null ||
            eyeLayer.sprite == null ||
            mouthLayer.sprite == null)
        {
            Debug.Log("MASK BELUM LENGKAP");
            return;
        }

        MaskType currentMask = baseLayer.sprite == maskPutih
            ? MaskType.Putih
            : MaskType.Hitam;

        EyeType currentEye = eyeLayer.sprite == eyeRetak
            ? EyeType.Retak
            : EyeType.Normal;

        MouthType currentMouth = MouthType.Flat;

        if (mouthLayer.sprite == mouthSenyum)
            currentMouth = MouthType.Senyum;
        else if (mouthLayer.sprite == mouthCemberut)
            currentMouth = MouthType.Cemberut;

        
        MaskNeeded result = DetectEmotion(
            currentMask,
            currentEye,
            currentMouth
        );

        NPCQueueManager.Instance.ServeCurrentNPC(result);

        MaskLayerController.Instance.resetLayer();
        CraftingManager.Instance.CloseCrafting();

        Debug.Log($"MASK DIKIRIM KE NPC: {result}");
    }



    void UpdateApplyButtonState()
    {
        bool isComplete =
            baseLayer.sprite != null &&
            eyeLayer.sprite != null &&
            mouthLayer.sprite != null;

        ApplyButton.interactable = isComplete;
    }


    private void Update()
    {
        UpdateApplyButtonState();
    }


    EmotionConfig GetConfig(MaskNeeded emotion)
    {
        foreach (var cfg in emotionConfigs)
        {
            if (cfg.emotion == emotion)
                return cfg;
        }
        return null;
    }

    public MaskNeeded DetectEmotion(
    MaskType mask,
    EyeType eye,
    MouthType mouth
)
    {
        foreach (var cfg in emotionConfigs)
        {
            if (cfg.mask == mask &&
                cfg.eye == eye &&
                cfg.mouth == mouth)
            {
                return cfg.emotion;
            }
        }

        return MaskNeeded.None; 
    }


}
