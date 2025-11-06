using Cinemachine;
using System.Collections;
using UnityEngine;
public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    [SerializeField] private float shakeForce = 1f;
    [SerializeField] private CinemachineVirtualCamera cam;
    private CinemachineImpulseListener impulseListener;
    private CinemachineImpulseDefinition impulseDefinition;
 
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


    public IEnumerator ZoomInCam()
    {
        cam.m_Lens.OrthographicSize = 5.8f;
        yield return new WaitForSeconds(0.15f);
        cam.m_Lens.OrthographicSize = 6f;
    }

}
