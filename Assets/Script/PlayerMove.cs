using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    enum attack { Running,Upper,}
    private Animator ani;
    private Rigidbody2D rig;
    private CapsuleCollider2D col;

    private bool isRun = false;

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


        Move();
    }

    private void Move()
    {
        if (isRun == true)
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * speed);
    }

    private void Attack()
    {
        ani.SetInteger("Attack",)
    }
}


