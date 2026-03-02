using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

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


}
