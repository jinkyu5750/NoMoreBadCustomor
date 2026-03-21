using NUnit.Framework.Constraints;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public DataManager dataManager;
    public bool isGameStarted { get; private set; } = false;
    public bool canLoadPlayScene { get; private set; } = false;

    public bool itemTestMode;

    public bool finishTutorial_Lobby { get; private set; } = false;
    public bool finishTutorial_Play { get; private set; } = false;

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
        dataManager.Init();

    }

    private void Start()
    {
        if(itemTestMode)
        {
            dataManager.playerData.shopData.AddItem_TestMode();
        }
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

    public void FinishTutorial_Lobby()
    {
        finishTutorial_Lobby = true;
    }
    public void FinishTutorial_Play()
    {
        finishTutorial_Play = true;
    }

}


