using System.Collections;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    CameraShakeZoom cameraShakeZoom;

    private Player player;
    public enum attack { Dash = 1, Upper, Lower }

    [SerializeField] private bool _canAttack = true;
    public bool canAttack { get { return _canAttack; } private set { _canAttack = value; } }

    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashPower;
    [SerializeField] private float jumpTime = 0.5f;
    [SerializeField] private float jumpPower;
    [SerializeField] private Vector2 attackBoxSize;

    public void InitPlayer(Player player)
    {
        this.player = player;
    }


    void Start()
    {
        cameraShakeZoom = GetComponent<CameraShakeZoom>();
    }


    public IEnumerator Attack(attack dir)
    {
        if (canAttack == false) yield break;

        canAttack = false;
        player.components.ani.SetInteger("Attack", (int)dir);
        
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
                    // player.isGround = false;

                    player.components.rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(jumpTime);
                //    player.components.rig.velocity = new Vector2(player.runSpeed, player.components.rig.velocity.y);
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
                break;
            case "Upper":

                break;
            case "Lower":

                break;

        }
        if (hit != null)
        {
            Debug.Log("¸Â¾Ò´Ù");
            StartCoroutine(cameraShakeZoom.ShakeCam(2.5f, 1, 0.1f));
            StartCoroutine(cameraShakeZoom.ZoomInCam());

            Vector2 randomCircle = Random.insideUnitCircle * 0.5f;
            // Instantiate(attackHitParticle, hit.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0), Quaternion.identity);
            ParticleManager.instance.UseObject("AttackHit", hit.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0));
            hit.gameObject.GetComponent<Enemy>().EnemyDead();

        }



    }


    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), attackBoxSize);
    }
}
