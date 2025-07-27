using System.Collections;
using UnityEngine;

public class PlayerHitDead : MonoBehaviour
{
    private Player player;
    CameraShakeZoom cameraShakeZoom;

    [SerializeField] private int _life = 3; 
    public int life { get { return _life; } private set { _life = value; } }
    public bool isDead { get; private set; } = false;
    void Start()
    {
        cameraShakeZoom = GetComponent<CameraShakeZoom>();
    }

    public void InitPlayer(Player player)
    {
        this.player = player;
    }

    public IEnumerator Hit(Collider2D col)
    {
        //튕겨나가서, 1초정도 있다가 일어나자
        // 스프라이트를 쪼개야할듯 등에
   
        player.components.sp.material.color = new Color(250f / 255f, 70f / 255f, 70f / 255f);
        life--;

        player.components.ani.SetTrigger("Hit");

        Vector2 hitDir = (transform.position - col.transform.position).normalized;
        player.components.rig.AddForce(hitDir * 14, ForceMode2D.Impulse);
        StartCoroutine(cameraShakeZoom.ShakeCam(2, 1, 0.2f));

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
