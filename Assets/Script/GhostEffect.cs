using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    private SpriteRenderer sp;

    private float ghostDelay;
    private float dashDelay = 0.05f;
    private float skillDashDelay = 0.02f;
    private float skillAttackDelay = 0.1f;

    private float curGhostDelay;

    private bool _isGhostOn;
    public bool IsGhostOn
    {
        set => _isGhostOn = value;
    }


    // 대쉬하면 Player스크립트에서 OnOff해주기 -> Update로 주기마다 본체의 스프라이트 동기화 후 Instantiate  -> Destroy(gameObject) 
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        curGhostDelay = ghostDelay;
    }

    public void SetDelay(string str)
    {
        switch(str)
        {
            case "Dash":
                ghostDelay = dashDelay;
                break;
            case "SkillDash":
                ghostDelay = skillDashDelay;
                break;
            case "SkillAttack":
                ghostDelay = skillAttackDelay;
                break;

        }
    }
    void Update()
    {

        if (ghostDelay <= 0)
        {
            Debug.Log("ghostDelay must be bigger then 0");
            return;
        }

        if (_isGhostOn == true)
        {

            if (curGhostDelay > 0)
                curGhostDelay -= Time.deltaTime;
            else
            {
                GameObject ghost = ParticleManager.instance.UseObject_GhostEffect("GhostEffect", transform.position);
                ghost.GetComponent<SpriteRenderer>().sprite = sp.sprite;
                curGhostDelay = ghostDelay;
            }
        }
  
    }


}
