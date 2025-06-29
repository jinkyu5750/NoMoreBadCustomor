using Cinemachine;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum attack { Dash = 1, Upper, Lower }

    private Animator ani;
    private Rigidbody2D rig;
    private CapsuleCollider2D col;
    private SpriteRenderer sp;

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
    [SerializeField] private GameObject attackHitParticle;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    private float max_runSpeed;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        runDust = transform.GetChild(0).gameObject;

    }

    private void Update()
    {

        if (isDead) return;

        if ((Input.GetKeyDown(KeyCode.D)))
        {
            isRunning = true;
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
            case attack.Dash:
                {
                    Instantiate(dashEffect, transform.position, transform.rotation);
                    GetComponent<GhostEffect>().IsGhostOn = true;
                    float curTime = dashTime;

                    rig.velocity = Vector2.right * dashPower;
                    while (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                        yield return null;
                    }
                    rig.velocity = new Vector2(runSpeed, rig.velocity.y);
                    GetComponent<GhostEffect>().IsGhostOn = false;
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
            Debug.Log("¸Â¾Ò´Ù");
            StartCoroutine(ShakeCam(2.5f, 1, 0.1f));
            StartCoroutine(ZoomInCam());

            Vector2 randomCircle = Random.insideUnitCircle * 0.5f;
            Instantiate(attackHitParticle,hit.transform.position + new Vector3(randomCircle.x,randomCircle.y,0), Quaternion.identity);
            hit.gameObject.GetComponent<Enemy>().EnemyDead();
            
        }



    }

    public IEnumerator Hit(Collider2D col)
    {

        isRunning = false; canAttack = false; sp.material.color = new Color(250f / 255f, 70f / 255f, 70f / 255f);
        life--;

        ani.SetTrigger("Hit");

        Vector2 hitDir = (transform.position - col.transform.position).normalized;
        rig.AddForce(hitDir * 15, ForceMode2D.Impulse);
        StartCoroutine(ShakeCam(2, 1, 0.2f));

        yield return new WaitForSeconds(0.5f);
        isRunning = true; canAttack = true; sp.material.color = Color.white;
    }

    public IEnumerator ShakeCam(float amplitude, float frequncy, float time)
    {
        CinemachineBasicMultiChannelPerlin per = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        per.m_AmplitudeGain = amplitude;
        per.m_FrequencyGain = frequncy;
        yield return new WaitForSeconds(time);
        per.m_AmplitudeGain = 0;
        per.m_FrequencyGain = 0;
    }

    public IEnumerator ZoomInCam()
    {
        //    cinemachine.m_Lens.OrthographicSize = Mathf.SmoothDamp(5f, 4.5f, 0.3f,1);
        yield return new WaitForSeconds(0.5f);
        cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(4.5f, 5, 0.5f);
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


