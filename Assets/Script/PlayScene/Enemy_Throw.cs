using DG.Tweening;
using UnityEngine;
public class Enemy_Throw : Enemy
{

    Animator ani;
    SpriteRenderer sp;

    [SerializeField] private bool isReverse;
    [SerializeField] private float coolTime = 3f;
    [SerializeField] private float curTime;

    Vector3 startPos, endPos; // enemy의 이동위치
    [SerializeField] private Vector2 detectSize;
    [SerializeField] private float throwPower;
    Collider2D target;
    Vector3 throwDir;
    Vector3 throwPos;
    private void Start()
    {
        ani = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        curTime = Random.Range(1, 2);

        startPos = transform.localPosition;
        if (isReverse)
            endPos = transform.localPosition + new Vector3(0, -1f, 0);
        else
            endPos = transform.localPosition + new Vector3(0, 1f, 0);

    }
    private void Update()
    {
        if (curTime <= 0)
            Attack();
        else
            curTime -= Time.deltaTime;
    }
    public void Attack()
    {
        target = Physics2D.OverlapBox(transform.position, detectSize, 0, LayerMask.GetMask("Player"));
        if (target != null)
        {
            curTime = Random.Range(coolTime - 0.5f, coolTime + 0.5f);
  
            if ((target.transform.position - transform.position).x < 0)
                sp.flipX = !isReverse;
            else
                sp.flipX = isReverse;

            transform.DOLocalMove(endPos, 1.5f).OnComplete(
                () =>
            ani.SetBool("Attack", true));
        }

    }

    public void Fire()
    {
        throwDir = (target.transform.position - transform.position) + new Vector3(0, 0.3f, 0);
        throwPos = transform.GetChild(0).GetComponent<Transform>().position; // 월드좌표

        GameObject thrownEnemy = ParticleManager.instance.UseObject_GhostEffect("ThrownEnemy", throwPos);

        Rigidbody2D rig = thrownEnemy.GetComponent<Rigidbody2D>();
        rig.AddForce(throwDir * throwPower, ForceMode2D.Impulse);
        rig.angularVelocity = Random.Range(400f, 700f);

    }
    public void SetAniBool()
    {
        ani.SetBool("Attack", false);
        transform.DOLocalMove(startPos, 1f);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, detectSize);
    }
}
