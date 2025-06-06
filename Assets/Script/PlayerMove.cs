using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum attack { Dash=1, Upper,Lower }

    private Animator ani;
    private Rigidbody2D rig;
    private CapsuleCollider2D col;

    private bool isRun = false;
    private bool canAttack = true;

    [SerializeField] private float runSpeed;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCurTime;
    [SerializeField] private float dashPower;

    private float max_runSpeed;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();


    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.D)))
        {
            ani.SetBool("IsRun", true);
            isRun = true;
        }

        if(Input.GetKeyDown(KeyCode.Space) && canAttack == true)
        {
            StartCoroutine(DashCoroutine());
             // Attack(attack.Dash);
        }


     //   if (dashCurTime <= 0) dashCurTime = dashTime;
        


        Move();
     
    }

    private void Move()
    {
        if (isRun == true)
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * runSpeed);
    }

    private void Attack(attack dir)
    {

        canAttack = false; 
        switch (dir)
        {
            case attack.Dash:
                ani.SetInteger("Attack", (int)attack.Dash);
                dashCurTime = dashTime;
                while (dashCurTime >= 0)
                {
                    dashCurTime -= Time.deltaTime;
                    rig.velocity = Vector2.right * dashPower;
                }
             //   rig.velocity = Vector2.zero;
                break;
            case attack.Upper:
                break;
            case attack.Lower:
                break;

        }
    }
    private IEnumerator DashCoroutine()
    {
        float time = dashTime;
        ani.SetInteger("Attack", 1);
        // ��� ���� ���� (����� ������, �ʿ��ϸ� ĳ���� ���⿡ �°� ����)
        Vector2 dashDirection = Vector2.right;

        // ��� ����
        rig.velocity = dashDirection * dashPower;

        // ���� �ð� ��� ����
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        // ��� ���� - ����
        rig.velocity = Vector2.zero;
        canAttack = true;
        
    }

    public void SetAttackAniParameter() 
    {
        canAttack = true;
        ani.SetInteger("Attack", 0);
    }
}


