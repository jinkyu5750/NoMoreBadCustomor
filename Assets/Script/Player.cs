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
    //isGround .. �� ������ �ɰŰ����� 
    //���������ϰ� �����ö� ����� -> isGround == false && canAttack == true;(getInteger(0)) 
    // �뽬���� -> isGround == true && canAttack == false
    // Run -> ���� �پ������鼭 �����Ҽ��ִ»���
    public Components components { get; private set; }

    public bool isGround; // ���Ŵ��


    [SerializeField] private float _runSpeed;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(playerAttack.Attack(PlayerAttack.attack.Dash));

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(playerAttack.Attack(PlayerAttack.attack.Upper));

        }

        if (playerHitDead.life == 0)
            playerHitDead.Dead();

        Move();

    }

    private void Move()
    {
        if (isGround && playerAttack.canAttack) // �ٴڿ� �پ������鼭 ������ ������ ����
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
        {
            playerAttack.SetCanAttack(false);
            StartCoroutine(playerHitDead.Hit(collision));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ParticleManager.instance.UseObject("LandingDust", transform.position);
            playerAttack.SetCurAttackCombo();
            isGround = true;
            components.ani.SetBool("IsGround", true);
            components.ani.SetBool("IsRun", true);
        }


    }

    private void OnCollisionExit2D(Collision2D collision) //�̰� ���� ����.. �۶� ���������ؼ�
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            components.ani.SetBool("IsGround", false);
            isGround = false;
        }
    }




}


