using System.Collections;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    CameraShakeZoom cameraShakeZoom;

    private Player player;
    public enum attack { Dash = 1, Upper, Lower }
    private attack curAttack;
    [SerializeField] private bool _canAttack = true;
    public bool canAttack { get { return _canAttack; } private set { _canAttack = value; } }

    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashPower;
    [SerializeField] private float jumpPower;

    [SerializeField] private Vector2 attackBoxSize;

    [SerializeField] private float g;
    [SerializeField] private float gravityScale;

    [SerializeField] private int max_Combo = 3;
    [SerializeField] private int curCombo = 0;
    public void InitPlayer(Player player)
    {
        this.player = player;
    }


    void Start()
    {
        cameraShakeZoom = GetComponent<CameraShakeZoom>();
    }


    private void Update()
    {
        if (player.components.rig.velocity.y < 0f)
        {
            if (curAttack == attack.Dash)
            {
 
                player.components.rig.gravityScale = 0;
            }
            else
            {
                player.components.rig.gravityScale = gravityScale;
            }
        }
        else
            player.components.rig.gravityScale = g;
    }
    public IEnumerator Attack(attack dir)
    {
        // 하고싶은것 : 같은 공격으로는 캔슬이 불가능, 다른 공격으로는 캔슬 가능 // 선입력 X , 타이밍맞춰서
        // canAttack이 켜지면 공격하되, 이전 공격이랑 같으면안됨, 최대3회

        if (canAttack == false || curAttack == dir || curCombo >= max_Combo) yield break;

        curCombo++;
        curAttack = dir;
        SetCanAttack(false, (int)dir);

        switch (dir)
        {
            case attack.Dash:
                {

                    ParticleManager.instance.UseObject("DashDust", transform.position);

                    GetComponent<GhostEffect>().IsGhostOn = true;
                    player.components.rig.velocity = Vector2.right * dashPower;

                    yield return new WaitForSeconds(dashTime);

                    player.components.rig.velocity = new Vector2(player.runSpeed, player.components.rig.velocity.y);
                    GetComponent<GhostEffect>().IsGhostOn = false;
                    break;
                }

            case attack.Upper:
                {

                    ParticleManager.instance.UseObject("DoubleJump", transform.position + new Vector3(0, -0.5f, 0));
                    player.components.rig.velocity = new Vector2(player.components.rig.velocity.x, 0);
                    player.components.rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);



                }
                break;
            case attack.Lower:
                break;

        }


    }

    public void AttackAniEvent(string attackDir)
    {

        Collider2D hit = new Collider2D();
        switch (attackDir)
        {
            case "Dash":
                hit = Physics2D.OverlapBox(transform.position + new Vector3(1, 1, 0), attackBoxSize, 0, LayerMask.GetMask("Enemy"));
                player.components.rig.gravityScale = g;
                break;
            case "Upper":
                hit = Physics2D.OverlapBox(transform.position + new Vector3(0, 2, 0), attackBoxSize, 0, LayerMask.GetMask("Enemy"));
                break;
            case "Lower":
                hit = Physics2D.OverlapBox(transform.position + new Vector3(0, 1, 0), attackBoxSize, 0, LayerMask.GetMask("Enemy"));
                break;

        }

        SetCanAttack(true, 0);

        if (hit != null)
        {

            StartCoroutine(cameraShakeZoom.ShakeCam(2.5f, 1, 0.1f));
            StartCoroutine(cameraShakeZoom.ZoomInCam());

            Vector2 randomCircle = Random.insideUnitCircle * 1f;
            ParticleManager.instance.UseObject("AttackHit", hit.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0));
            hit.gameObject.GetComponent<Enemy>().EnemyDead();
        }

    }



    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }
    public void SetCanAttack(bool canAttack, int i)
    {
        this.canAttack = canAttack;
        player.components.ani.SetInteger("Attack", i);

    }
    public void SetCurAttackCombo()
    {
        curCombo = 0;
        curAttack = 0;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 2f, 0), attackBoxSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 1, 0), attackBoxSize);
    }
}
