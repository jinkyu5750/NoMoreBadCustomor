using System.Collections;
using UnityEngine;

public class Onigiri : Enemy
{
    [SerializeField] float speed;
    [SerializeField]Rigidbody2D onigiri; // 
    protected override void Start()
    {
       base.Start();
        rig.velocity = Vector3.left * speed;
        onigiri = transform.GetChild(0).GetComponent<Rigidbody2D>();

        onigiri.velocity = Vector3.left * speed;
        onigiri.angularVelocity = Random.Range(300f, 600f); // ºù±Ûºù±Û
    }

}
