using Cinemachine;
using System.Collections;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{

    private Player player;

    public enum attack { Dash = 1, Upper, Lower, Skill, Additional }
    [SerializeField] private bool _canAttack = true;
    public bool canAttack { get { return _canAttack; } private set { _canAttack = value; } }

    [SerializeField] private float dashTime = 0.4f;
    [SerializeField] private float dashPower = 10;
    private float jumpPower = 8;
    private float dropPower = 10;


    private float g = 1.3f;
    private float gravityScale = 3.3f;

    private int max_Combo = 2;
    private int curCombo = 0;
    [SerializeField] private GameObject comboGaneSkillGageEffect;
    [SerializeField] CameraShakeProfile attackProfile;
    [SerializeField] CameraShakeProfile groundSlamProfile;
    [SerializeField] CameraShakeProfile additionalAttackProfile;

    private CinemachineImpulseSource impulseSource;

    [SerializeField] CinemachineVirtualCamera skillCam;
    private float skillGage = 0;
    public bool isSkill { get; private set; } = false;
    private bool nextAttack_Skill = false;
    private int skillAttackCount = 3;
    public float slowFactor = 0.05f;
    public float slowLength = 1f;

    [SerializeField] private AttackData dashAttackData;
    [SerializeField] private AttackData upperAttackData;
    [SerializeField] private AttackData lowerAttackData;
    [SerializeField] private AttackData skillAttackData;
    [SerializeField] private AttackData additionalAttackData;
    [SerializeField] private AttackData curAttackData;

    private bool canAdditionalAttack = false;

    public int combo { get; private set; } = 0;
    private CircleCollider2D magnetCol;
    public void InitPlayer(Player player)
    {
        this.player = player;
    }

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        magnetCol = transform.Find("MagnetRange").GetComponent<CircleCollider2D>();
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
            if (curAttackData == dashAttackData || curAttackData == additionalAttackData)
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

        switch (dir)
        {
            case attack.Dash:
                curAttackData = dashAttackData;
                break;
            case attack.Upper:
                curAttackData = upperAttackData;
                break;
            case attack.Lower:
                curAttackData = lowerAttackData;
                break;

        }

        SetCanAttack(0);
        player.components.ani.SetInteger("Attack", (int)dir);

        if (!player.components.ani.GetBool("IsGround"))
            curCombo++;


        switch (dir)
        {
            case attack.Dash:
                {

                    ParticleManager.instance.UseObject("DashDust", transform.position, Quaternion.identity);
                    SoundManager.instance.PlaySFX("DashWhoosh");

                    GetComponent<GhostEffect>().SetDelay("Dash");
                    GetComponent<GhostEffect>().IsGhostOn = true;
                    player.components.rig.velocity = Vector2.right * dashPower;

                    yield return new WaitForSeconds(dashTime);

                    if (curAttackData != dashAttackData) // 중간에 피격당한경우
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
                    SoundManager.instance.PlaySFX("UpperJump");
                    ParticleManager.instance.UseObject("DoubleJump", transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                    player.components.rig.velocity = new Vector2(player.components.rig.velocity.x, 0);
                    player.components.rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);



                }
                break;
            case attack.Lower:
                {
                    if (player.components.ani.GetBool("IsGround") == true)
                    {

                        player.components.ani.SetInteger("Attack", 0);
                        curAttackData = null;
                        SetCanAttack(1);
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
    public void GaneSkillGage(string name) // Receipt랑 공격 수치조절필요
    {
        if (isSkill) return;

        // 5에 몇퍼센트를 하나.. 미미한디
        //   skillGage += 5 + (5 * GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(3)) + Random.Range(-3, 5); // 100까지 대략 6~7회
        skillGage += 134 + Random.Range(-3, 5); // 100까지 대략 6~7회

        /*   switch (name)
           {
               case "Attack":
                      skillGage += 3 + (3 * GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(3)) + Random.Range(-3, 5); 
                   break;
               case "Receipt":
                   skillGage += 0.1f;
                   break;
               case "Combo":
                   skillGage += 5f;
                   break;
           }*/
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

    public Vector3 GetEnemyPos(Collider2D enemy)
    {
        if (enemy == null) return Vector3.zero;

        SpriteRenderer enemySp = enemy.GetComponent<SpriteRenderer>();
        float x = enemySp.bounds.min.x + 0.15f;
        float y = enemySp.bounds.center.y - 0.45f;

        return new Vector3(x, y, 0);
    }
    public void UseSkill_Button()
    {
        StartCoroutine(UseSkill());
    }
    IEnumerator UseSkill()
    {
        if (skillGage < 100 || !player.components.ani.GetBool("IsGround") || !canAttack) yield break;


        curAttackData = skillAttackData;
        isSkill = true;
        SetCanAttack(0);
        skillGage = 0;
        int attackCount =  skillAttackCount +GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(2);
        UIManager.Instance.UpdateSkillGage(0);
        UIManager.Instance.ResetSkillGageBar();
        UIManager.Instance.MoveSkillPanel(true);
        SoundManager.instance.PlaySFX("MovePanel");
        skillCam.Priority = 11;

        magnetCol.enabled = true;
        player.components.hurtCol.enabled = false;
        player.components.rig.velocity = Vector3.zero;
        player.components.ani.SetBool("SkillOn", true); //선동작

        // 여기까지 UI세팅과 필요한 변수 세팅

        yield return new WaitForSeconds(2.5f);
        player.components.ani.SetTrigger("SkillStart");
        GetComponent<GhostEffect>().IsGhostOn = true;

        //공격시작
        for (int i = 0; i < attackCount; i++)
        {
            nextAttack_Skill = false;
            GetComponent<GhostEffect>().SetDelay("SkillDash");
            int skillNum = Random.Range(1, 6);


            Collider2D enemy = GetClosestEnemy();
            if (enemy == null) break;

            Vector3 enemyPos = GetEnemyPos(enemy);
            CalCamAngle(enemyPos);

            player.components.ani.SetTrigger("SkillDash");
            yield return new WaitForSeconds(0.15f); // Dash 애니메이션으로의 트랜지션 딜레이
            SoundManager.instance.PlaySFX("SkillDashWhoosh");
            while (Vector3.Distance(transform.position, enemyPos) > 0.3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyPos, 80f * Time.deltaTime);
                yield return null;
            }

            player.components.rig.velocity = Vector3.zero;
            player.components.ani.SetInteger("SkillNum", skillNum);
            GetComponent<GhostEffect>().SetDelay("SkillAttack");

            yield return new WaitForSeconds(0.5f);
            player.components.ani.SetInteger("SkillNum", 0);

            DoSlowMotion(slowFactor);
            yield return new WaitUntil(() => nextAttack_Skill == true);

        }


        player.components.hurtCol.enabled = true;
        player.components.ani.SetBool("SkillOn", false);
        GetComponent<GhostEffect>().IsGhostOn = false;
        isSkill = false;
        magnetCol.enabled = false;
        UIManager.Instance.MoveSkillPanel(false);
        skillCam.Priority = 9;
        skillCam.m_Lens.Dutch = 0;


    }
    public void DoSlowMotion(float slowFactor)
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
        Collider2D enemy = GetClosestEnemy();
        Vector3 enemyPos = GetEnemyPos(enemy);

        if (!canAttack || !canAdditionalAttack || enemy == null)
        {
            if (enemy == null)
                canAdditionalAttack = false;

            yield break;
        }

        SetCanAttack(0);
        curAttackData = additionalAttackData;

        player.components.ani.SetInteger("AdditionalAttack", 1); // 추격
        player.components.hurtCol.enabled = false;

        while (Vector3.Distance(transform.position, enemyPos) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyPos, 60f * Time.deltaTime);
            yield return null;
        }
        player.components.rig.velocity = Vector3.zero;

        player.components.ani.SetInteger("AdditionalAttack", 2); // 공격

    }
    #region 타격을 위한 메소드(히트박스) // 애니메이션 이벤트 
    public void AttackAniEvent(string attackDir)
    {
        player.components.ani.SetInteger("Attack", 0);

        if (attackDir != "Lower") // Lower은 애니메이션이벤트로 따로처리      
            SetCanAttack(1);


        switch (attackDir)
        {
            case "Dash":
                StartCoroutine(AttackHitbox(dashAttackData.hitBoxPos, dashAttackData.hitBoxSize, 0.3f));
                SoundManager.instance.PlaySFX("DashSlash");
                player.components.rig.gravityScale = g;
                break;
            case "Upper":
                StartCoroutine(AttackHitbox(upperAttackData.hitBoxPos, upperAttackData.hitBoxSize, 0.3f));
                SoundManager.instance.PlaySFX("UpperSlash");
                break;
            case "Lower":
                StartCoroutine(AttackHitbox(lowerAttackData.hitBoxPos, lowerAttackData.hitBoxSize, 1f));
                break;
            case "Skill":
                StartCoroutine(AttackHitbox(skillAttackData.hitBoxPos, skillAttackData.hitBoxSize, 0.3f));
                break;
            case "AdditionalAttack":
                StartCoroutine(AttackHitbox(additionalAttackData.hitBoxPos, additionalAttackData.hitBoxSize, 0.3f));
                player.components.ani.SetInteger("AdditionalAttack", 0);
                player.components.rig.velocity = Vector2.right * player.runSpeed;
                player.components.rig.gravityScale = g;
                player.components.hurtCol.enabled = true;
                Collider2D col = Physics2D.OverlapBox(transform.position + new Vector3(0, -0.5f, 0), additionalAttackData.hitBoxSize, 0, LayerMask.GetMask("Ground"));
                if (col != null) player.components.ani.SetBool("AddiToJump", false);
                else player.components.ani.SetBool("AddiToJump", true);
                break;

        }


    }
    IEnumerator AttackHitbox(Vector3 posOffset, Vector2 boxSize, float duration)
    {
        Vector3 pos = transform.position + posOffset;

        float elapsed = 0f;
        while (elapsed < duration)
        {

            Collider2D hit = null;

            if (curAttackData == lowerAttackData) // 하단공격은 캐릭터를따라 쭉 포지션 업데이트
                hit = Physics2D.OverlapBox(transform.position + posOffset, boxSize, 0, LayerMask.GetMask("Enemy"));
            else
                hit = Physics2D.OverlapBox(pos, boxSize, 0, LayerMask.GetMask("Enemy"));
            //여기까지 몹감지


            if (hit != null)
            {
                SoundManager.instance.PlaySFX(curAttackData.sfxName);
                UIManager.Instance.SetComboUI(++combo);
                if (combo % 10 == 0)
                {
                    SoundManager.instance.PlaySFX("GaneSkillGage");
                    GameObject go = Instantiate(comboGaneSkillGageEffect);
                    go.transform.SetParent(transform, false);
                    go.transform.localPosition = new Vector3(0, 0.5f, 0);
                    Destroy(go,2f);
                    GaneSkillGage("Combo");
                }

                StartCoroutine(hit.gameObject.GetComponent<Enemy>().EnemyDead());
                // -> 적 Dead

                CameraShakeProfile profile = curAttackData.shakeProfile;
                CameraManager.instance.ShakeCameraFromProfile(profile, hit.gameObject.GetComponent<CinemachineImpulseSource>());
                // StartCoroutine(CameraManager.instance.ZoomInCam());
                // -> 카메라 핸들

                Vector2 randomCircle = Random.insideUnitCircle * 1f;
                ParticleManager.instance.UseObject(curAttackData.particleName, hit.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0), Quaternion.identity);
                // -> 히트 파티클

                if (curAttackData != skillAttackData)
                    GaneSkillGage("Attack");
                ScoreManager.instance.MonsterScore(isSkill);
                // -> 스코어,스킬게이지 증가

                if (GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(1) == 1)
                {
                    if (curAttackData.canTriggerAdditionalAttack)
                        canAdditionalAttack = true;      //트랜지션문제는 트랜지션설정에서 해결하자~    
                    else
                        canAdditionalAttack = false;

                }
                // -> 추가타 관련


                // 한 메소드에서 너무 여러기능을 한다지만.. 따로 뺴는것도 애매함 한두줄짜린데 메소드만 몇억개될거같음 
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
        SoundManager.instance.PlaySFX("GroundSlam");
        ParticleManager.instance.UseObject("GroundSlam", transform.position, Quaternion.identity);
    }

    public void SetCanAttack(int canAttack)
    {
        this.canAttack = canAttack == 1;
    }

    public void SetCurAttackData() // DashAttack은 0으로 안해주면 공중에서 쭉 중력0됨
    {
        curAttackData = null;
    }

    public void SetnextAttack_Skill()
    {
        nextAttack_Skill = true;
    }
    public void HitDuringAttack()
    {
        //   if (curAttackData != dashAttackData) return;

        player.components.ani.SetInteger("Attack", 0);
        canAttack = false;
        curAttackData = null;

    }

    #endregion

    public void SetComboZero()
    {
        this.combo = 0;
    }
    void OnDrawGizmos()
    {
        /*    Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
            Gizmos.DrawWireCube(transform.position + new Vector3(0, 2f, 0), attackBoxSize);
            Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.3f, 0), attackBoxSize + new Vector2(2, 3));

            Gizmos.DrawWireCube(transform.position + new Vector3(12.5f, 0.5f, 0), new Vector3(25, 20, 0));*/
    }






}
