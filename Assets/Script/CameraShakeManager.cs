using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{

    public static CameraShakeManager instance;

    [SerializeField] private float shakeForce = 1f;
    [SerializeField] private CinemachineImpulseListener impulseListener;
    private CinemachineImpulseDefinition impulseDefinition;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

/*    public void ShakeCamera(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }
*/
    public void ShakeCameraFromProfile(CameraShakeProfile profile,CinemachineImpulseSource impulseSource)
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
}
