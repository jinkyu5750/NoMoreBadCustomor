using Cinemachine;
using System.Collections;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{

    private Player player;
    public enum attack { Dash = 1, Upper, Lower }
    private attack curAttack;
    [SerializeField] private bool _canAttack = true;
    public bool canAttack { get { return _canAttack; } private set { _canAttack = value; } }

    private float dashTime = 0.4f;
    private float dashPower = 10;
    private float jumpPower = 8;
    private float dropPower = 10;

    private Vector2 attackBoxSize = new Vector2(1.7f, 2);

    private float g = 1.3f;
    private float gravityScale = 3.3f;

     private int max_Combo = 2;
     private int curCombo = 0;

    [SerializeField] CameraShakeProfile attackProfile;
    [SerializeField] CameraShakeProfile groundSlamProfile;
    private CinemachineImpulseSource impulseSource;
    public void InitPlayer(Player player)
    {
        this.player = player;
    }

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (player.components.rig.velocity.y < 0f)
        {
            if (curAttack == attack.Dash)
                player.components.rig.gravityScale = 0;
            else
                player.components.rig.gravityScale = gravityScale;

        }
        else
            player.components.rig.gravityScale = g;


        if (player.components.ani.GetBool("IsGround"))
            curCombo = 0;
    }
    public IEnumerator Attack(attack dir)
    {

        if (!canAttack || curCombo >= max_Combo) yield break;

        curAttack = dir; // 직전했던 공격은 못한다는 컨셉 폐기...

        SetCanAttack(0);
        player.components.ani.SetInteger("Attack", (int)dir);

        if (!player.components.ani.GetBool("IsGround"))
            curCombo++;


        switch (dir)
        {
            case attack.Dash:
                {

                    ParticleManager.instance.UseObject("DashDust", transform.position, Quaternion.identity);

                    GetComponent<GhostEffect>().IsGhostOn = true;
                    player.components.rig.velocity = Vector2.right * dashPower;

                    yield return new WaitForSeconds(dashTime);

                    player.components.rig.velocity = new Vector2(player.runSpeed, player.components.rig.velocity.y);
                    GetComponent<GhostEffect>().IsGhostOn = false;
                    break;
                }

            case attack.Upper:
                {

                    ParticleManager.instance.UseObject("DoubleJump", transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                    player.components.rig.velocity = new Vector2(player.components.rig.velocity.x, 0);
                    player.components.rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);



                }
                break;
            case attack.Lower:
                {
                    if (player.components.ani.GetBool("IsGround") == true)
                    {
                        curAttack = 0;
                        SetCanAttack(1);
                        player.components.ani.SetInteger("Attack", 0);
                        yield break;
                    }

                    ParticleManager.instance.UseObject("DoubleJump", transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(new Vector3(0, 0, 180f)));
                    player.components.rig.velocity = Vector2.zero;
                    player.components.rig.AddForce(Vector2.down * dropPower, ForceMode2D.Impulse);
                }
                break;

        }


    }

    public void AttackAniEvent(string attackDir)
    {

        if (attackDir != "Lower") // Lower은 애니메이션이벤트로 따로처리
        {
            SetCanAttack(1);
            player.components.ani.SetInteger("Attack", 0);
        }
        Debug.Log("한번");
        Collider2D hit = null;
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
                CameraManager.instance.ShakeCameraFromProfile(groundSlamProfile, impulseSource);
                Debug.Log("한번");
                ParticleManager.instance.UseObject("GroundSlam", transform.position, Quaternion.identity);
                break;


        }

        if (hit != null)
        {

            CameraManager.instance.ShakeCameraFromProfile(attackProfile,hit.gameObject.GetComponent<CinemachineImpulseSource>());
            StartCoroutine(CameraManager.instance.ZoomInCam());

            Vector2 randomCircle = Random.insideUnitCircle * 1f;
            ParticleManager.instance.UseObject("AttackHit", hit.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0), Quaternion.identity);
       //     hit.gameObject.GetComponent<Enemy>().EnemyDead();
        }

    }


    public void SetCanAttack(int canAttack)
    {
        this.canAttack = canAttack == 1;
    }


    public void SetCurAttack() // DashAttack 애니메이션이벤트용
    {
        curAttack = 0;
    }


    /*    void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
            Gizmos.DrawWireCube(transform.position + new Vector3(0, 2f, 0), attackBoxSize);
            Gizmos.DrawWireCube(transform.position + new Vector3(0, 1, 0), attackBoxSize);
        }*/
}
