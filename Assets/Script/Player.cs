using Cinemachine;
using NUnit.Framework.Constraints;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;

public class Player : MonoBehaviour
{
    enum attack { Dash = 1, Upper, Lower }

    private Animator ani;
    private Rigidbody2D rig;
    private CapsuleCollider2D col;

    private bool canAttack = true;
    private bool isDead = false;
    private bool isRunning = false;

    [SerializeField] private float runSpeed;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashPower;
    [SerializeField] private int life = 3;

    [SerializeField] private GameObject dashEffect;
    private GameObject runDust;
    [SerializeField] private Vector2 attackBoxSize;

    [SerializeField] CinemachineVirtualCamera cinemachine;
    private float max_runSpeed;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        runDust = transform.GetChild(0).gameObject;
      
    }

    private void Update()
    {

        if (isDead) return;

        if ((Input.GetKeyDown(KeyCode.D)))
        {
            isRunning =true ;
            ani.SetBool("IsRun", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canAttack == true)
        {
            StartCoroutine(Attack(attack.Dash));

        }

        if (life == 0)
            Dead();

        Move();

    }

    private void Move()
    {
        if (isRunning)
        {
            rig.velocity = new Vector2(1, 0) * runSpeed;
            runDust.SetActive(true);
        }
        else
            runDust.SetActive(false);
    }


    private IEnumerator Attack(attack dir)
    {
        isRunning = false;
        canAttack = false;
        ani.SetInteger("Attack", (int)dir);
     
        switch (dir)
        {      
            case attack.Dash: //트레일넣을까말까..
                {
                    Instantiate(dashEffect, transform.position, transform.rotation);
                    float curTime = dashTime;

                    rig.velocity = Vector2.right * dashPower;
                    while (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                        yield return null;
                    }
                    rig.velocity = new Vector2(runSpeed, rig.velocity.y);                             
                    break;
                }

            case attack.Upper:
                break;
            case attack.Lower:
                break;

        }

        yield return new WaitForSeconds(0.5f);
        canAttack = true;
        isRunning = true;
        ani.SetInteger("Attack", 0);
    }
 
    public void AttackAniEvent(string attackDir)
    {

        Collider2D hit = new Collider2D();
        switch (attackDir)
        {
            case "Dash":
                hit = Physics2D.OverlapBox(transform.position + new Vector3(1, 1, 0), attackBoxSize, 0, LayerMask.GetMask("Enemy"));
                break;
            case "Upper":

                break;
            case "Lower":

                break;

        }
        if (hit != null)
        {
            Debug.Log("맞았다");
            hit.gameObject.GetComponent<Enemy>().EnemyDead();
        }



    }

    public IEnumerator Hit(Collider2D col)
    {

        isRunning = false; canAttack = false;
        life--;

        ani.SetTrigger("Hit");
        
        CinemachineBasicMultiChannelPerlin per = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        per.m_AmplitudeGain = 1;
        per.m_FrequencyGain = 3;
        Vector2 hitDir = (transform.position - col.transform.position).normalized;
        rig.AddForce(hitDir * 13, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        per.m_AmplitudeGain = 0;
        per.m_FrequencyGain = 0;
        yield return new WaitForSeconds(0.3f);
        isRunning = true;   canAttack = true;
    }
    private void Dead()
    {
        isDead = true;
        ani.SetTrigger("Dead");
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
            StartCoroutine(Hit(collision));  
       
    }




    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
    }

}


