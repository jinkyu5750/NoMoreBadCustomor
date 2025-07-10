using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    Camera_Shake_Zoom camera_Shake_Zoom;

    private Player player;
    public enum attack { Dash = 1, Upper, Lower }
    [SerializeField] private bool canAttack = true; 
    
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashPower;

    [SerializeField] private Vector2 attackBoxSize;
    
    public void InitPlayer(Player player)
    {
        this.player = player;
    }
    
    
    void Start()
    {
        camera_Shake_Zoom = GetComponent<Camera_Shake_Zoom>();
    }

   
    public IEnumerator Attack(attack dir)
    {
        if (canAttack == false) yield break;


        player.components.ani.SetInteger("Attack", (int)dir);

        switch (dir)
        {
            case attack.Dash:
                {
              
                    ParticleManager.instance.UseObject("DashDust",transform.position);
                    GetComponent<GhostEffect>().IsGhostOn = true;
                    float curTime = dashTime;

                    player.components.rig.velocity = Vector2.right * dashPower;
                    while (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                        yield return null;
                    }
                    player.components.rig.velocity = new Vector2(player.runSpeed, player.components.rig.velocity.y);
                    GetComponent<GhostEffect>().IsGhostOn = false;
                    break;
                }

            case attack.Upper:
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
            StartCoroutine(camera_Shake_Zoom.ShakeCam(2.5f, 1, 0.1f));
            StartCoroutine(camera_Shake_Zoom.ZoomInCam());

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
