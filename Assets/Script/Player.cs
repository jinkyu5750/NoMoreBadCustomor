using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    enum attack { Dash = 1, Upper, Lower }

    private Animator ani;
    private Rigidbody2D rig;
    private CapsuleCollider2D col;

    private bool canAttack = true;
    private bool isDead = false;
    [SerializeField] private float runSpeed;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashPower;
    private int life = 3;

    [SerializeField] private GameObject dashEffect;

    [SerializeField] private Vector2 attackBoxSize;

    private BoxCollider2D[] attackCol = new BoxCollider2D[3];
    private BoxCollider2D upperAttackCol;
    private BoxCollider2D lowerAttackCol;
    private float max_runSpeed;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        attackCol = GetComponentsInChildren<BoxCollider2D>();
    }

    private void Update()
    {

        if (isDead) return;

        if ((Input.GetKeyDown(KeyCode.D)))
        {
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
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * runSpeed);
    }

    private IEnumerator Attack(attack dir)
    {
        canAttack = false;
        ani.SetInteger("Attack", (int)dir);
        Collider2D hit;
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
                    hit = Physics2D.OverlapBox(transform.position+ new Vector3(1,1,0), attackBoxSize, 0, LayerMask.GetMask("Enemy"));
                    break;
                }

            case attack.Upper:
                break;
            case attack.Lower:
                break;
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
    }

    private void Dead()
    {
        isDead = true;
        ani.SetTrigger("Dead");
    }
    /*   public void SetAttackAniParameter()
       {
           canAttack = true;
           ani.SetInteger("Attack", 0);
       }
   */


    public void OnOffAttackCol(string attackDir)
    {
        StartCoroutine(OnOffAttackColCoroutine(attackDir));
    }
    public IEnumerator OnOffAttackColCoroutine(string attackDir)
    {
        switch (attackDir)
        {
            case "Dash": //attackCol[0].enabled = isOn ? true : false; 삼항연산자떠올리고 기발하다 ㅋㅋ 생각했는데 =isOn이 더 기발하네.. 근데 파라미터 두개는 못씀 에바
                attackCol[0].enabled = true;
                yield return new WaitForSeconds(0.3f);
                attackCol[0].enabled = false;
                break;
            case "Upper":
                attackCol[1].enabled = true;
                yield return null;
                attackCol[1].enabled = false;
                break;
            case "Lower":
                attackCol[2].enabled = true;
                yield return null; 
                attackCol[2].enabled = false;
                break;

        }

        canAttack = true;
        ani.SetInteger("Attack", 0);

    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag.Equals("Enemy"))
        {

            Vector2 hitDir = (transform.position - collision.transform.position).normalized;
            rig.AddForce(hitDir * 5, ForceMode2D.Impulse);
            life--;

        }

       
    }
}


