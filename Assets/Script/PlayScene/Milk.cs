using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : Enemy
{

    Animator ani;
    ParticleSystem particle;
    [SerializeField] private float attackCoolTime;
    private float attackCurTime;

    protected override void Start()
    {
        base.Start();
        ani = GetComponent<Animator>();
        particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        attackCurTime = attackCoolTime;
    }

    private void Update()
    {
        if (attackCurTime >= 0)
            attackCurTime -= Time.deltaTime;
        else
            milkAttack();
    }
    public void milkAttack()
    {
        attackCurTime = attackCoolTime;

        ani.SetBool("Attack", true);
    }

    public IEnumerator AttackAniEvent()
    {
        particle.Play();
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("Attack", false);
        particle.Stop();
    }

}
