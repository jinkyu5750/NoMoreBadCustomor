using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneScript : MonoBehaviour
{
    
    void Update()
    {
        if(Input.anyKeyDown)
        {
            LoadingManager.instance.LoadScene("IntroScene");
        }
    }
}
