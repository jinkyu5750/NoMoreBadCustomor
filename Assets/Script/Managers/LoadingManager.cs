using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    public static LoadingManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingManager>();

                if (obj != null)
                    instance = obj;
                else
                    Debug.Log("LodingManager 없음!!!");
            }

            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private TextMeshProUGUI loadingGage;

    private GameObject loadingImage;
    private GameObject lobbyToPlayFade;

    public string loadSceneName;
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        progressBar = GetComponentInChildren<Slider>();
        canvasGroup = GetComponent<CanvasGroup>();
        loadingGage = progressBar.GetComponentInChildren<TextMeshProUGUI>();

        loadingImage = transform.Find("Loading...").gameObject;
        lobbyToPlayFade = transform.Find("LobbyToPlayFade").gameObject;

    }
    public void LoadScene(string sceneName,bool isLodingBarLoad)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded; // 이게머지??
        loadSceneName = sceneName;

        if(isLodingBarLoad)
        {
            StartCoroutine(LoadScene_LoadingBar());
        }
        else
        {
            StartCoroutine(LoadScene_Fade());
        }

    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    private IEnumerator LoadScene_LoadingBar()
    {
        SoundManager.instance.StopBGM();
        SwitchLoadingImage(true);
        progressBar.value = 0;

        yield return StartCoroutine(Fade(true));

        AsyncOperation async = SceneManager.LoadSceneAsync(loadSceneName);
        async.allowSceneActivation = false;

        float timer = 0.0f;

        while (!async.isDone)
        {
            yield return null;

            if (async.progress < 0.9f)
            {
                progressBar.value = async.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = Mathf.Lerp(0.9f, 1.0f, timer);

                if (progressBar.value >= 1.0f)
                {
                    SoundManager.instance.PlayBGM("LobbyBGM");
                    async.allowSceneActivation = true;
                    yield break;
                }
            }

            loadingGage.text = Mathf.FloorToInt(progressBar.value * 100).ToString() + "%";
        }
    }
    public IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0.0f;

        while (timer <= 1.0f)
        {
            yield return null;

            timer += Time.unscaledDeltaTime * 3.0f;

            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0.0f, 1.0f, timer) : Mathf.Lerp(1.0f, 0.0f, timer);
        }

        /*        if (!isFadeIn)
                    gameObject.SetActive(false);*/
    }

    public IEnumerator DelayedFade(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(Fade(true));
    }
    public IEnumerator LoadScene_Fade()
    {

        if (!GameManager.Instance.isGameStarted) yield break;

        // yield return StartCoroutine(Fade(true)); Fade는 DelayedFade를 이용

        AsyncOperation async = SceneManager.LoadSceneAsync(loadSceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            yield return null;

            if (async.progress >= 0.9f)
            {
                if (GameManager.Instance.canLoadPlayScene)
                {
                    SoundManager.instance.PlayBGM("PlayBGM" + Random.Range(1, 3));
                    Fade(false);
                    async.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    public void SwitchLoadingImage(bool isLoadingImage)
    {
        loadingImage.gameObject.SetActive(isLoadingImage);
        lobbyToPlayFade.gameObject.SetActive(!isLoadingImage);
    }
}
