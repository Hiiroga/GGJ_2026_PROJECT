using UnityEngine;
using UnityEngine.UI;

public class Mute : ButtonBaseClass
{
    [SerializeField] private Sprite[] VolumeImage; //0 unmute, 1 mute
    private bool _mute;
    private Image ObjectImage;
    void Awake()
    {
        _mute = false;        
        if(TryGetComponent<Image>(out Image img))
        {
            ObjectImage = img;
            ObjectImage.sprite = VolumeImage[0];
        }
    }

    void Start()
    {
        MusicManager.Instance.Play("mainmenu");
    }
    public override void OnClick()
    {
        _mute = !_mute;
        AudioListener.volume = _mute ? 0 : 1;
        ObjectImage.sprite = _mute ? VolumeImage[1] : VolumeImage[0];
    }
}