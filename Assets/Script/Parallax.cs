using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private Camera cam;
    [SerializeField] private float parallaxPower;

    private int oldPlatform, newPlatform;
    void Start()
    {
        cam = Camera.main;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxPower);
        float dist = (cam.transform.position.x * parallaxPower);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;

            newPlatform = Random.Range(1, 4);
            ParticleManager.instance.UseObject($"Platform{newPlatform}", new Vector3(startPos, transform.position.y, 0f),Quaternion.identity);
        }
        else if (temp < startPos - length) 
        {
            startPos -= length;

            int num = Random.Range(0, 3);
            ParticleManager.instance.UseObject($"Platform{newPlatform}", new Vector3(startPos, transform.position.y, 0f), Quaternion.identity);
        }
    }
}
