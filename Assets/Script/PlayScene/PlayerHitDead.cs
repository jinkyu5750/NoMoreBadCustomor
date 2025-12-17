using System.Collections;
using UnityEngine;

public class PlayerHitDead : MonoBehaviour
{
    private Player player;
    [SerializeField] private CameraShakeProfile hitProfile;
    [SerializeField] float knockbackPower;
    [SerializeField] float hitCoolTime = 1;
    float hitCurTime = 0;
    [SerializeField] private int life_Max = 3;
    [SerializeField] private int _life;
    public int life { get { return _life; } private set { _life = value; } }
    public bool isDead { get; private set; } = false;



    private void Start()
    {
        life = life_Max;
    }
    private void Update()
    {
        if (hitCurTime > 0)
        {
            hitCurTime -= Time.deltaTime;
        }
    }
    public void InitPlayer(Player player)
    {
        this.player = player;
    }

    public IEnumerator Hit(Collider2D col)
    {

        if (hitCurTime > 0) yield break;

        hitCurTime = hitCoolTime;

        GetComponent<PlayerAttack>().HitDuringDash();

        player.components.sp.material.color = new Color(250f / 255f, 70f / 255f, 70f / 255f);
        life--;
        UIManager.Instance.UpdateHPBar((float)life / life_Max);
        player.components.ani.SetTrigger("Hit");


        Vector2 hitDir = Vector2.left;
        player.components.rig.velocity = Vector2.zero;
        player.components.rig.velocity = hitDir * knockbackPower;

        // CameraManager.instance.ShakeCameraFromProfile(hitProfile,col.gameObject.GetComponent<CinemachineImpulseSource>());


        yield return new WaitForSeconds(0.5f);
        player.components.sp.material.color = Color.white;
        yield return new WaitForSeconds(1f);
        player.components.ani.SetTrigger("WakeUp");

    }

    public IEnumerator Fall()
    {
        //  if (hitCurTime > 0) yield break; // 맞아서 튕겨져나갔을때 처리해야돼

        hitCurTime = hitCoolTime;

        GetComponent<PlayerAttack>().HitDuringDash();


        player.components.ani.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);

        Collider2D spawnPoint = Physics2D.OverlapBox(transform.position, new Vector2(30, 30), 0,LayerMask.GetMask("SpawnPoint")); // 위치가 영.. 
          if (spawnPoint == null) // 플랫폼이 동시에 사라졌을때 처리
              spawnPoint = Physics2D.OverlapBox(transform.position, new Vector2(50, 50),0, LayerMask.GetMask("SpawnPoint"));

        transform.position = spawnPoint.transform.position + new Vector3(-10,10,0); 
        player.components.rig.velocity = Vector2.down;
        yield return new WaitForSeconds(1f);
        player.components.ani.SetTrigger("WakeUp");
        life--;
        UIManager.Instance.UpdateHPBar((float)life / life_Max);

    }


    public void Dead()
    {
        isDead = true;
        player.components.ani.SetTrigger("Dead");
    }
}
