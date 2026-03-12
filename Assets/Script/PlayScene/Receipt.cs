using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receipt : MonoBehaviour
{

   private Transform player;
   [SerializeField] private bool startMagnet = false;
   [SerializeField] float accel = 3f;
   [SerializeField] float magnetSpeed = 2.5f;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (!startMagnet) return;

        Vector3 dir = (player.position - transform.position + Vector3.up).normalized;

        magnetSpeed += accel * Time.deltaTime;

        transform.position += dir * magnetSpeed * Time.deltaTime;
    }

 

    public void SetStartMagnet()
    {
        startMagnet= true;
    }
}
