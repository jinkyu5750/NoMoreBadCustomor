using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum attack { Running=1, Upper,Lower }

    private Animator ani;
    private Rigidbody2D rig;
    private CapsuleCollider2D col;

    private bool isRun = false;
    private bool isAttack = false;

    [SerializeField] private float speed;
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

        if(Input.GetKeyDown(KeyCode.Space) && ani.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
              Attack(attack.Running);

        }

        Move();
     
    }

    private void Move()
    {
        if (isRun == true)
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * speed);
    }

    private void Attack(attack dir)
    {
      
        switch (dir)
        {
           case attack.Running:
                ani.SetInteger("Attack", (int)attack.Running);
                break;
            case attack.Upper:
                break;
            case attack.Lower:
                break;

        }
    }
}


