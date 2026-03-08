using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public enum SoundType { BGM = 1, SFX = 2 }
    public static SoundManager instance;

    [SerializeField] private AudioMixer audioMixer;
    private AudioSource BGMsource;
    private AudioSource SFXsource;

    [SerializeField] private AudioClip[] BGMclips;
    [SerializeField] private AudioClip[] SFXclips;

    Dictionary<string, AudioClip> BGMdic = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> SFXdic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


        foreach (AudioClip clip in BGMclips)
        {
            BGMdic.Add(clip.name, clip);
        }
        foreach (AudioClip clip in SFXclips)
        {
            SFXdic.Add(clip.name, clip);
        }
    }

    private void Start()
    {
        BGMsource = transform.Find("BGM").GetComponent<AudioSource>();
        SFXsource = transform.Find("SFX").GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SFXsource.PlayOneShot(SFXdic["Click"]);
    }
    public void PlayBGM(string name)
    {
        BGMsource.clip = BGMdic[name];
        BGMsource.Play();
    }

    public void PauseBGM()
    {
        if (BGMsource.isPlaying)
            BGMsource.Pause();
        else
            BGMsource.Play();
    }
    public void StopBGM()
    {
        BGMsource.Stop();
    }
    public void PlaySFX(string name)
    {
        SFXsource.PlayOneShot(SFXdic[name]);
    }

    public void SetVolume(SoundType type,float value)
    {
        audioMixer.SetFloat(type.ToString(), Mathf.Log10(value) * 20);
    }

    public void SetFatalSound(bool isFatal)
    {
        if (isFatal)
        {
            audioMixer.SetFloat("LowPassCutOff", 975f);
            audioMixer.SetFloat("LowPassCutOff", 975f);

            audioMixer.SetFloat("LowPassCutOff", 975f);

        }
    }
}
