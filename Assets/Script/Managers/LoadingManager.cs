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

    }
    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded; // 이게머지??
        loadSceneName = sceneName;

        StartCoroutine(StartLoadScene());
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    private IEnumerator StartLoadScene()
    {
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
                    async.allowSceneActivation = true;   
                    yield break;
                }
            }

            loadingGage.text = Mathf.FloorToInt(progressBar.value * 100).ToString() + "%";
        }
    }
    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0.0f;

        while (timer <= 1.0f)
        {
            yield return null;

            timer += Time.unscaledDeltaTime * 3.0f;

            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0.0f, 1.0f, timer) : Mathf.Lerp(1.0f, 0.0f, timer);
        }

        if (!isFadeIn)
            gameObject.SetActive(false);
    }
}
