using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public SoundManager.SoundType type;
    public Slider slider;
    public Button muteButton;
    public bool isMute;
    public float savedValue;
}
public class SoundSetting : MonoBehaviour
{
    [SerializeField]Sound BGM;
    [SerializeField]Sound SFX; // Sound배열로 하면 더 좋지만.. 그냥 당장은 BGM,SFX라는 이름으로 구분하고싶어서..
    Sound s = null;

    [SerializeField] Sprite non_MuteImage;
    [SerializeField] Sprite muteImage;

    private void Start()
    {
        BGM.slider.onValueChanged.AddListener((value) => SetVolume(SoundManager.SoundType.BGM, value));
        SFX.slider.onValueChanged.AddListener((value) => SetVolume(SoundManager.SoundType.SFX, value));

        BGM.muteButton.onClick.AddListener(() => Mute(SoundManager.SoundType.BGM));
        SFX.muteButton.onClick.AddListener(() => Mute(SoundManager.SoundType.SFX));

    }


    public void SetVolume(SoundManager.SoundType type, float value)
    {

        if (type == SoundManager.SoundType.BGM)
            s = BGM;
        else if (type == SoundManager.SoundType.SFX)
            s = SFX;


        if (s.isMute)
        {
            s.isMute = false;
            s.muteButton.image.sprite = non_MuteImage;
        }

        value = value / 10f;
        value = Mathf.Clamp(value, 0.0001f, 1f);
        SoundManager.instance.SetVolume(type, value);
    }

    public void Mute(SoundManager.SoundType type)
    {

        if (type == SoundManager.SoundType.BGM)
            s = BGM;
        else if (type == SoundManager.SoundType.SFX)
            s = SFX;

        if (s.isMute)
        {
            s.muteButton.GetComponent<Image>().sprite = non_MuteImage;
            s.isMute = false;
            s.slider.SetValueWithoutNotify(s.savedValue);
            s.savedValue = s.savedValue / 10f;
            SoundManager.instance.SetVolume(type, s.savedValue);
        }
        else if (!s.isMute)
        {
            s.muteButton.GetComponent<Image>().sprite = muteImage;
            s.isMute = true;
            s.savedValue = s.slider.value;
            s.slider.SetValueWithoutNotify(0f);
            SoundManager.instance.SetVolume(type, 0.0001f);

        }


    }
}
