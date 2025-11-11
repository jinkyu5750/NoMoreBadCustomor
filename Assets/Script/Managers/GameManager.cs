using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 
    public static GameManager Instance;

    public bool isGameStarted { get; private set; } = false;
    public bool canLoadPlayScene { get; private set; } = false;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void StartGame()
    {
        if (isGameStarted) return;

        isGameStarted = true;
        Debug.Log("Game Started!");
    }

    public void LoadGame()
    {
        if(canLoadPlayScene) return;

        canLoadPlayScene = true;
        LoadingManager.instance.LoadScene("PlayScene");

        Debug.Log("Load GameScene Started!");

    }

}


