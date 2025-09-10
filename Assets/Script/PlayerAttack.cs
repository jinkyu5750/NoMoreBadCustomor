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


    private float skillGage = 0;
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

        switch (attackDir)
        {
            case "Dash":
                StartCoroutine(AttackHitbox(transform.position + new Vector3(1, 1, 0), 0.3f));
                player.components.rig.gravityScale = g;
                break;
            case "Upper":
                StartCoroutine(AttackHitbox(transform.position + new Vector3(0, 2, 0), 0.3f));
                break;
            case "Lower":
                StartCoroutine(AttackHitbox(transform.position + new Vector3(0, 0.3f, 0), 1f)); // 하단범위  . . . 조정이 필요할수도 
                break;

        }


    }
    IEnumerator AttackHitbox(Vector3 pos, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Collider2D hit = Physics2D.OverlapBox(pos, attackBoxSize, 0, LayerMask.GetMask("Enemy"));
            if (hit != null)
            {

                CameraManager.instance.ShakeCameraFromProfile(attackProfile, hit.gameObject.GetComponent<CinemachineImpulseSource>());
                StartCoroutine(CameraManager.instance.ZoomInCam());

                Vector2 randomCircle = Random.insideUnitCircle * 1f;
                ParticleManager.instance.UseObject("AttackHit", hit.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0), Quaternion.identity);
                hit.gameObject.GetComponent<Enemy>().EnemyDead();
                GaneSkillGage();
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    public void GaneSkillGage()
    {
        skillGage += 925 + Random.Range(-3, 5); // 100까지 대략 6~7회
        UIManager.Instance.UpdateSkillGage(skillGage);
    }
    public void UseSkill()
    {
        if (skillGage < 100) return;

        skillGage = 0;
        UIManager.Instance.UpdateSkillGage(0);
        UIManager.Instance.ResetSkillGageBar();
        //쇽샥샥샥쇽샥
    }

    public void GroundSlamEffect()
    {
        CameraManager.instance.ShakeCameraFromProfile(groundSlamProfile, impulseSource);
        ParticleManager.instance.UseObject("GroundSlam", transform.position, Quaternion.identity);
    }
    public void SetCanAttack(int canAttack)
    {
        this.canAttack = canAttack == 1;
    }


    public void SetCurAttack() // DashAttack 애니메이션이벤트용
    {
        curAttack = 0;
    }

    /*
            void OnDrawGizmos()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 2f, 0), attackBoxSize);
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.3f, 0), attackBoxSize);
            }*/
}
