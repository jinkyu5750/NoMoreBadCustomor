using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackData", fileName = "AttackData")]
public class AttackData : ScriptableObject
{
    public string particleName;
    public string sfxName;
    public CameraShakeProfile shakeProfile;
    public bool canTriggerAdditionalAttack;
    public Vector3 hitBoxPos;
    public Vector3 hitBoxSize;

}
