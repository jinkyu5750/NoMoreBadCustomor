using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    // Resume,Quit,Settings

 
    public void ClickResumeButton()
    {
        UIManager.Instance.MenuPanel(false);
    }

    public void ClickQuitButton()
    {
        UIManager.Instance.MenuPanel(false);
        LoadingManager.instance.LoadScene("LobbyScene", true);
    }

    public void ClickSettingsButton()
    {

    }
}
