using System.Collections;
using UnityEngine;

public class Components
{
    public Rigidbody2D rig;
    public Animator ani;
    public CapsuleCollider2D col;
    public SpriteRenderer sp;

}
public class Player : MonoBehaviour
{

    public Components components { get; private set; }
  
    public bool isRunning = false; // 제거대상


    [SerializeField ]private float _runSpeed;
    public float runSpeed { get { return _runSpeed; } private set { _runSpeed = value; } }

    private PlayerAttack playerAttack;
    private PlayerHitDead playerHitDead;

    private GameObject runDust;

    private float max_runSpeed;
    private void Start()
    {
        components = new Components()
        {
            rig = GetComponent<Rigidbody2D>(),
            ani = GetComponent<Animator>(),
            col = GetComponent<CapsuleCollider2D>(),
            sp = GetComponent<SpriteRenderer>()
        };

        runDust = transform.GetChild(0).gameObject;

        playerAttack = GetComponent<PlayerAttack>();
        playerHitDead = GetComponent<PlayerHitDead>();
        playerAttack.InitPlayer(this);
        playerHitDead.InitPlayer(this);

    }

    private void Update()
    {

        if (playerHitDead.isDead) return;

        if ((Input.GetKeyDown(KeyCode.D)))
        {
            isRunning = true;
            components.ani.SetBool("IsRun", true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(playerAttack.Attack(PlayerAttack.attack.Dash));

        }

        if (playerHitDead.life == 0)
            playerHitDead.Dead();

        Move();

    }

    private void Move()
    {
        if (isRunning)
        {
            components.rig.velocity = new Vector2(1, 0) * runSpeed;
            runDust.SetActive(true);
        }
        else
            runDust.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
            StartCoroutine(playerHitDead.Hit(collision));

    }


  


}


