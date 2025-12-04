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




    public void Dead()
    {
        isDead = true;
        player.components.ani.SetTrigger("Dead");
    }
}
