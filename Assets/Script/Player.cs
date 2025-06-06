using System.Collections;
using UnityEditor;
using UnityEngine;


public class Player : MonoBehaviour
{
    enum attack { Dash=1, Upper,Lower }

    private Animator ani;
    private Rigidbody2D rig;
    private CapsuleCollider2D col;

    private bool canAttack = true;
    private bool isDead = false;
    [SerializeField] private float runSpeed;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashPower;

    [SerializeField] private GameObject dashEffect;

    private float max_runSpeed;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();


    }

    private void Update()
    {

        if (isDead) return;

        if ((Input.GetKeyDown(KeyCode.D)))
        {
            ani.SetBool("IsRun", true);
        }

        if(Input.GetKeyDown(KeyCode.Space) && canAttack == true)
        {
            StartCoroutine(Attack(attack.Dash));
     
        }

        Move();
     
    }

    private void Move()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Run")) 
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * runSpeed);
    }

    private IEnumerator Attack(attack dir)
    {
        canAttack = false;
        ani.SetInteger("Attack", (int)dir);

        switch (dir)
        {  
            case attack.Dash: //∆Æ∑π¿œ≥÷¿ª±Ó∏ª±Ó..
                {
                    Instantiate(dashEffect, transform.position, transform.rotation);
                    float curTime = dashTime;

                    rig.velocity = Vector2.right * dashPower;
                    while (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                        yield return null;
                    }
                    rig.velocity = new Vector2(runSpeed , rig.velocity.y);

                    break;
                }

            case attack.Upper:
                break;
            case attack.Lower:
                break;
        }

    }

    private void Dead()
    {
        isDead = true;
        ani.SetBool("Dead", true);
    }
    public void SetAttackAniParameter() 
    {
        canAttack = true;
        ani.SetInteger("Attack", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("»Ï§ª§ª");

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("»Ï");
            Vector2 hitDir = collision.transform.position - transform.position;
            rig.AddForce(hitDir * 2, ForceMode2D.Impulse);
            Dead();

        }
    }
}


