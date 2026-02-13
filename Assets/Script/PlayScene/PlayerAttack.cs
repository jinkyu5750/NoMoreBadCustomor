using Cinemachine;
using System.Collections;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{

    private Player player;

    public enum attack { Dash = 1, Upper, Lower, Skill, Additional }
    [SerializeField] private attack curAttack;
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


    [SerializeField] CinemachineVirtualCamera skillCam;
    private float skillGage = 0;
    private bool isSkill = false;
    private bool nextAttack_Skill = false;
    private int skillattackCount = 3;
    public float slowFactor = 0.05f;
    public float slowLength = 1f;

    private bool canAdditionalAttack = false;
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


        if (Time.timeScale >= slowFactor) // Time.timeScale이 slowFactor 이상인 경우 즉, 슬로우모션적용 시 // 때문에 timeScale을 0으로 맞추는건 영향 안갈듯
        {
            Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        if (isSkill)
        {
            player.components.rig.gravityScale = 0;
            return;
        }

        if (player.components.rig.velocity.y < 0f)
        {
            if (curAttack == attack.Dash || curAttack == attack.Additional)
                player.components.rig.gravityScale = 0;
            else
                player.components.rig.gravityScale = gravityScale;

        }
        else
            player.components.rig.gravityScale = g;



        if (player.components.ani.GetBool("IsGround"))
            curCombo = 0;
    }


    #region 기본공격 
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

                    GetComponent<GhostEffect>().SetDelay("Dash");
                    GetComponent<GhostEffect>().IsGhostOn = true;
                    player.components.rig.velocity = Vector2.right * dashPower;

                    yield return new WaitForSeconds(dashTime);

                    if (curAttack != attack.Dash) // 중간에 피격당한경우
                    {
                        GetComponent<GhostEffect>().IsGhostOn = false;
                        yield break;
                    }

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

    #endregion


    #region 스킬
    public void GaneSkillGage()
    {
        if (isSkill) return;

        // 5에 몇퍼센트를 하나.. 미미한디
        //   skillGage += 5 + (5 * GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(3)) + Random.Range(-3, 5); // 100까지 대략 6~7회
        skillGage += 134 + Random.Range(-3, 5); // 100까지 대략 6~7회

        UIManager.Instance.UpdateSkillGage(skillGage);

    }

    public Collider2D GetClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position + new Vector3(12.5f, 0.5f, 0), new Vector3(25, 20, 0), 0, LayerMask.GetMask("Enemy"));// 몹감지
        Collider2D enemy = null;
        float minDistance = float.MaxValue;
        foreach (Collider2D c in enemies)
        {
            float distance = Vector3.Distance(transform.position, c.transform.position);
            if (distance < minDistance)
            {
                enemy = c;
                minDistance = distance;
            }
        }

        return enemy;
    }

    public void UseSkill_Button()
    {
        StartCoroutine(UseSkill());
    }
    IEnumerator UseSkill()
    {
        if (skillGage < 100 || !player.components.ani.GetBool("IsGround") || !canAttack) yield break;

        curAttack = attack.Skill;
        isSkill = true;
        SetCanAttack(0);
        skillGage = 0;
        skillattackCount += GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(2);
        UIManager.Instance.UpdateSkillGage(0);
        UIManager.Instance.ResetSkillGageBar();
        UIManager.Instance.MoveSkillPanel(true);
        skillCam.Priority = 11;

        player.components.col.enabled = false;
        player.components.rig.velocity = Vector3.zero;
        player.components.ani.SetBool("SkillOn", true); //선동작

        // 여기까지 UI세팅과 필요한 변수 세팅

        yield return new WaitForSeconds(3f);
        player.components.ani.SetTrigger("SkillStart");
        GetComponent<GhostEffect>().IsGhostOn = true;

        //공격시작
        for (int i = 0; i < skillattackCount; i++)
        {
            nextAttack_Skill = false;
            GetComponent<GhostEffect>().SetDelay("SkillDash");
            int skillNum = Random.Range(1, 6);


            Collider2D enemy = GetClosestEnemy();
            if (enemy == null) break;

            Vector3 enemyPos = enemy.transform.position;
            enemyPos -= new Vector3(0, 0.5f, 0); //타격 위치보정
            CalCamAngle(enemyPos);

            yield return new WaitForSeconds(0.15f); // Dash 애니메이션으로의 트랜지션 딜레이
            while (Vector3.Distance(transform.position, enemyPos) > 1f)
            {
                player.components.rig.velocity = (enemyPos - transform.position).normalized * 45f;
                yield return null;
            }

            player.components.rig.velocity = Vector3.zero;
            player.components.ani.SetInteger("SkillNum", skillNum);
            GetComponent<GhostEffect>().SetDelay("SkillAttack");

            yield return new WaitForSeconds(0.5f);
            player.components.ani.SetInteger("SkillNum", 0);

            DoSlowMotion();
            yield return new WaitUntil(() => nextAttack_Skill == true);

        }


        // 가까운 플랫폼 스폰포인트로... 
        player.components.col.enabled = true;
        player.components.ani.SetBool("SkillOn", false);
        GetComponent<GhostEffect>().IsGhostOn = false;
        isSkill = false;
        UIManager.Instance.MoveSkillPanel(false);
        skillCam.Priority = 9;
        skillCam.m_Lens.Dutch = 0;


    }
    public void DoSlowMotion()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void CalCamAngle(Vector3 enemyPos)
    {
        float angle = Mathf.Atan2(enemyPos.y, enemyPos.x) * Mathf.Rad2Deg;
        skillCam.m_Lens.Dutch = angle;
    }


    #endregion 스킬종료 후 착지 어케해야할까.. 끝에 바닥에 떨어지는걸.. 어카지


    public IEnumerator AdditionalAttack()
    {

        if (!canAttack || !canAdditionalAttack) yield break;
        SetCanAttack(0);

        curAttack = attack.Additional;

        Collider2D enemy = GetClosestEnemy();
        Vector3 enemyPos = enemy.transform.position;

        player.components.ani.SetInteger("AdditionalAttack", 1); // 추격
        player.components.col.enabled = false;

        while (Vector3.Distance(transform.position, enemyPos) > 1f)
        {
            player.components.rig.velocity = (enemyPos - transform.position).normalized * 45f;
            yield return null;
        }
        player.components.rig.velocity = Vector3.zero;

        player.components.ani.SetInteger("AdditionalAttack", 2); // 공격
        canAdditionalAttack = false;
        curAttack = 0;

    }
    #region 타격을 위한 메소드(히트박스) // 애니메이션 이벤트 
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
                StartCoroutine(AttackHitbox(new Vector3(1, 1, 0), attackBoxSize, 0.3f));
                player.components.rig.gravityScale = g;
                break;
            case "Upper":
                StartCoroutine(AttackHitbox(new Vector3(0, 2, 0), attackBoxSize, 0.3f));
                break;
            case "Lower":
                StartCoroutine(AttackHitbox(new Vector3(0, 0.3f, 0), attackBoxSize, 1f));
                break;
            case "Skill":
                StartCoroutine(AttackHitbox(new Vector3(1, 1, 0), attackBoxSize + new Vector2(5, 5), 0.3f));
                break;
            case "AdditionalAttack":
                StartCoroutine(AttackHitbox(new Vector3(1, 1, 0), attackBoxSize, 0.3f));
                player.components.ani.SetInteger("AdditionalAttack", 0);
                player.components.rig.gravityScale = g;
                player.components.col.enabled = true;
                break;

        }


    }
    IEnumerator AttackHitbox(Vector3 posOffset, Vector2 boxSize, float duration)
    {
        Vector3 pos = transform.position + posOffset;

        float elapsed = 0f;
        while (elapsed < duration)
        {

            Collider2D hit = new Collider2D();

            if (curAttack == attack.Lower) // 하단공격은 캐릭터를따라 쭉 포지션 업데이트
                hit = Physics2D.OverlapBox(transform.position + posOffset, attackBoxSize, 0, LayerMask.GetMask("Enemy"));
            else
                hit = Physics2D.OverlapBox(pos, attackBoxSize, 0, LayerMask.GetMask("Enemy"));

            if (hit != null)
            {

                StartCoroutine(hit.gameObject.GetComponent<Enemy>().EnemyDead());

                CameraManager.instance.ShakeCameraFromProfile(attackProfile, hit.gameObject.GetComponent<CinemachineImpulseSource>());
                StartCoroutine(CameraManager.instance.ZoomInCam());

                Vector2 randomCircle = Random.insideUnitCircle * 1f;
                ParticleManager.instance.UseObject("AttackHit", hit.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0), Quaternion.identity);

                GaneSkillGage();
                ScoreManager.instance.MonsterScore(isSkill);

                if (curAttack != attack.Skill || curAttack != attack.Additional)
                    canAdditionalAttack = true;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

    }
    #endregion


    #region 애니메이션 이벤트용
    public void GroundSlamEffect()
    {
        CameraManager.instance.ShakeCameraFromProfile(groundSlamProfile, impulseSource);
        ParticleManager.instance.UseObject("GroundSlam", transform.position, Quaternion.identity);
    }

    public void SetCanAttack(int canAttack)
    {
        this.canAttack = canAttack == 1;
    }

    public void SetCurAttack() // DashAttack은 0으로 안해주면 공중에서 쭉 중력0됨
    {
        curAttack = 0;
    }

    public void SetnextAttack_Skill()
    {
        nextAttack_Skill = true;
    }
    public void HitDuringDash()
    {
        if (curAttack != attack.Dash) return;

        player.components.ani.SetInteger("Attack", 0);
        canAttack = false;
        curAttack = 0;

    }

    #endregion
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        /*  Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
          Gizmos.DrawWireCube(transform.position + new Vector3(0, 2f, 0), attackBoxSize);
          Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.3f, 0), attackBoxSize + new Vector2(2, 3));*/

        Gizmos.DrawWireCube(transform.position + new Vector3(12.5f, 0.5f, 0), new Vector3(25, 20, 0));
    }
}
