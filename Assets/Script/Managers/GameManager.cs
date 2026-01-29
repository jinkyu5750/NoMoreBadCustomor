using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public DataManager dataManager;
    public bool isGameStarted { get; private set; } = false;
    public bool canLoadPlayScene { get; private set; } = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);

        dataManager = new DataManager();

    }

    private void Update()
    {
        Debug.Log(dataManager);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LobbyScene")
        {
            isGameStarted = false;
            canLoadPlayScene = false;
        }
    }
    public void StartGame()
    {
        if (isGameStarted) return;

        isGameStarted = true;
        Debug.Log("Game Started!");
    }

    public void LoadGame()
    {
        if (canLoadPlayScene) return;

        canLoadPlayScene = true;
        LoadingManager.instance.LoadScene("PlayScene", false);

        Debug.Log("Load GameScene Started!");

    }

}


