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
    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxPower);
        float dist = (cam.transform.position.x * parallaxPower);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length) 
        {
            startPos -= length;
        }
    }
}
