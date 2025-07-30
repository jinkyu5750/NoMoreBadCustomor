using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CameraShake/New Profile")] 
public class CameraShakeProfile : ScriptableObject
{
    [Header("Impulse Source Settings")]
    public float impulseForce = 1f;
    public float impulseTime = 0.2f;
    public Vector3 defaultVelocity = new Vector3(0,-1f,0);
    public AnimationCurve impulseCurve;

    [Header("Impulse Listener Settings")]
    public float listenerAmplitude = 1f;
    public float listenerFrequncy = 1f;
    public float listenerDuration = 1f;




}