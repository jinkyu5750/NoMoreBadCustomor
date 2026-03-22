using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoStream : MonoBehaviour
{

    VideoPlayer video;
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "GrokVideo.mp4");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
