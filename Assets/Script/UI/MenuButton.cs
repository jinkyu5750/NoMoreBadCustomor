using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    // Resume,Quit,Settings

 
    public void ClickResumeButton()
    {
        UIManager.Instance.HideMenuPanel();
    }

    public void ClickQuitButton()
    {
        UIManager.Instance.HideMenuPanel();
        LoadingManager.instance.LoadScene("LobbyScene");
    }

    public void ClickSettingsButton()
    {

    }
}
