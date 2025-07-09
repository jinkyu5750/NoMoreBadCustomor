using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shake_Zoom : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera cinemachine;
    public IEnumerator ShakeCam(float amplitude, float frequncy, float time)
    {
        CinemachineBasicMultiChannelPerlin per = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        per.m_AmplitudeGain = amplitude;
        per.m_FrequencyGain = frequncy;
        yield return new WaitForSeconds(time);
        per.m_AmplitudeGain = 0;
        per.m_FrequencyGain = 0;
    }

    public IEnumerator ZoomInCam()
    {
        //    cinemachine.m_Lens.OrthographicSize = Mathf.SmoothDamp(5f, 4.5f, 0.3f,1);
        yield return new WaitForSeconds(0.5f);
        cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(4.5f, 5, 0.5f);
    }
}
