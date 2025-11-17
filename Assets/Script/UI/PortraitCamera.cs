using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitCamera : MonoBehaviour
{

    [SerializeField] private Camera PortraitCam;
    [SerializeField] private Transform player;

    void LateUpdate()
    {
        Vector3 camPos = new Vector3 (player.position.x, player.position.y +1.3f, -10f);
        PortraitCam.transform.position = camPos ;
    }
}
