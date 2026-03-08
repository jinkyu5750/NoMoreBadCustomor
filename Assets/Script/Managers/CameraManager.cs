using Cinemachine;
using System.Collections;
using System.Threading;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    [SerializeField] private float zoomInTime = 2f;
    [SerializeField] private float targetOrthoSize = 3f;

    [SerializeField] private float shakeForce = 1f;
    [SerializeField] private CinemachineVirtualCamera cam;
    private CinemachineImpulseListener impulseListener;
    private CinemachineImpulseDefinition impulseDefinition;
    CinemachineFramingTransposer transposer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        impulseListener = cam.GetComponent<CinemachineImpulseListener>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ShakeCameraFromProfile(CameraShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        SetupCameraShakeSettings(profile, impulseSource);
        impulseSource.GenerateImpulseWithForce(profile.impulseForce);
    }

    public void SetupCameraShakeSettings(CameraShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        //ImpulseSource Settings
        impulseDefinition.m_ImpulseDuration = profile.impulseTime;
        impulseSource.m_DefaultVelocity = profile.defaultVelocity;
        impulseDefinition.m_CustomImpulseShape = profile.impulseCurve;

        //ImpulseListener Settings
        impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequncy;
        impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;



    }


    public IEnumerator ZoomInOutCam()
    {
        float startOrthoSize = cam.m_Lens.OrthographicSize;
        Vector3 offset = transposer.m_TrackedObjectOffset;
        transposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);

        yield return StartCoroutine(ZoomCam(startOrthoSize, targetOrthoSize, zoomInTime));
        yield return StartCoroutine(ZoomCam(targetOrthoSize, startOrthoSize, zoomInTime / 2));
    //    transposer.m_TrackedObjectOffset = offset;

    }


    public IEnumerator ZoomCam(float startSize,float targetSize,float time)
    {

        float curTime = 0f;
        while (curTime <= time) 
        {

            curTime += Time.deltaTime;
            float t = curTime / time;
            cam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }
    }
}
