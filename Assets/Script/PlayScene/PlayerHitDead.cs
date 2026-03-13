using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHitDead : MonoBehaviour
{
    private Player player;
    private PlayerAttack playerAttack;
    [SerializeField] private CameraShakeProfile hitProfile;
    [SerializeField] float knockbackPower;
    [SerializeField] float hitCoolTime = 3;
    float hitCurTime = 0;
    [SerializeField] private int life_Max = 3;
    [SerializeField] private int _life;
    public int life { get { return _life; } private set { _life = value; } }
    public bool isDead { get; private set; } = false;

    bool oneMoreLife = true;
    bool isFatal = false;
    [SerializeField] private Volume volume;
     float addedIntensity = 0.1f;
 
    private void Start()
    {
        life_Max += GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(0);
        life = life_Max;
    }
    private void Update()
    {
        if (hitCurTime > 0)
        {
            hitCurTime -= Time.deltaTime;
        }

        if (life == 1 && !isFatal)
        {
            isFatal = true;
            StartCoroutine(Fatal());
        }
    }
    public void InitPlayer(Player player, PlayerAttack playerAttack)
    {
        this.player = player;
        this.playerAttack = playerAttack;
    }

    public IEnumerator Hit(Collider2D col)
    {

        if (hitCurTime > 0) yield break;
        hitCurTime = hitCoolTime;

        playerAttack.SetComboZero();
        UIManager.Instance.SetComboUI(0);
        player.isHit = true;
        playerAttack.SetCanAttack(0);
        playerAttack.HitDuringAttack();

        player.components.sp.material.color = new Color(250f / 255f, 70f / 255f, 70f / 255f);
        life--;
        UIManager.Instance.UpdateHPBar((float)life / life_Max);
        SoundManager.instance.PlaySFX("PlayerHit");
        player.components.ani.SetTrigger("Hit");


        Vector2 hitDir = Vector2.left;
        player.components.rig.velocity = Vector2.zero;
        player.components.rig.velocity = hitDir * knockbackPower;

        // CameraManager.instance.ShakeCameraFromProfile(hitProfile,col.gameObject.GetComponent<CinemachineImpulseSource>());


        yield return new WaitForSeconds(0.5f);
        player.components.sp.material.color = Color.white;
        player.components.ani.SetBool("WakeUp", true);
        yield return new WaitForSeconds(0.5f);
        player.components.ani.SetBool("WakeUp", false);
        playerAttack.SetCanAttack(1);
        player.isHit = false;

    }

    public IEnumerator Fall()
    {
        //  if (hitCurTime > 0) yield break; // ¸Â¾Æ¼­ Æ¨°ÜÁ®³ª°¬À»¶§ Ã³¸®ÇØ¾ßµÅ

        if (GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(4) == 1 && oneMoreLife)
        {

            oneMoreLife = false;

            SoundManager.instance.PlaySFX("OneMoreLife");

            player.components.rig.velocity = Vector3.zero;
            player.components.col.enabled = false;
            player.components.ani.SetBool("Spin", true);
            player.components.rig.velocity = Vector3.up * 17;

            yield return new WaitForSeconds(1.5f);
            player.components.col.enabled = true;
            player.components.ani.SetBool("Spin", false);
            player.components.rig.velocity = new Vector2(1, 0) * player.runSpeed;

            yield break;
        }


        hitCurTime = hitCoolTime;

        playerAttack.HitDuringAttack();
        playerAttack.SetComboZero();
        UIManager.Instance.SetComboUI(0);

        player.components.ani.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);

        transform.position = new Vector3(transform.position.x - 10f, 5f, 0);
        player.components.rig.velocity = Vector2.down;
        yield return new WaitForSeconds(1f);
        player.components.ani.SetTrigger("WakeUp");
        life--;
        UIManager.Instance.UpdateHPBar((float)life / life_Max);

    }


    public IEnumerator Dead()
    {
        StartCoroutine(CameraManager.instance.ZoomInOutCam());
        playerAttack.DoSlowMotion(0.1f);

        isDead = true;
        player.components.ani.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);
        UIManager.Instance.ResultPanel(true);
    }

    public IEnumerator Fatal()
    {

        SoundManager.instance.SetFatalSound(true);

        Vignette vignette;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            while (true)
            {
                vignette.intensity.value = 0.5f + Mathf.PingPong(Time.time * 0.2f, addedIntensity);
                yield return null;
            }

        }

        FilmGrain filmGrain;
        if (volume.profile.TryGet<FilmGrain>(out filmGrain))
        {
            filmGrain.intensity.value = 0.5f + addedIntensity;
        }

    }
}
