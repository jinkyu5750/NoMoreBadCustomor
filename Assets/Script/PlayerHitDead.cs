using System.Collections;
using UnityEngine;

public class PlayerHitDead : MonoBehaviour
{
    private Player player;
    Camera_Shake_Zoom camera_Shake_Zoom;

    [SerializeField] private int _life = 3; 
    
    
    public int life { get { return _life; } private set { _life = value; } }
    public bool isDead { get; private set; } = false;
    void Start()
    {
        camera_Shake_Zoom = GetComponent<Camera_Shake_Zoom>();
    }

    public void InitPlayer(Player player)
    {
        this.player = player;
    }

    public IEnumerator Hit(Collider2D col)
    {

        player.components.sp.material.color = new Color(250f / 255f, 70f / 255f, 70f / 255f);
        life--;

        player.components.ani.SetTrigger("Hit");

        Vector2 hitDir = (transform.position - col.transform.position).normalized;
        player.components.rig.AddForce(hitDir * 15, ForceMode2D.Impulse);
        StartCoroutine(camera_Shake_Zoom.ShakeCam(2, 1, 0.2f));

        yield return new WaitForSeconds(0.5f);
        player.components.sp.material.color = Color.white;
    }


    public void Dead()
    {
        isDead = true;
        player.components.ani.SetTrigger("Dead");
    }
}
