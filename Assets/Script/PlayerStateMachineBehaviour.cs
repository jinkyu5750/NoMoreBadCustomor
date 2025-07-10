using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachineBehaviour : StateMachineBehaviour
{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        // 달리기 -> 공격,피격
        animator.GetComponent<Player>().isRunning = false;
        animator.GetComponent<PlayerAttack>().SetCanAttack(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

   
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //공격,피격 -> 달리기
        animator.GetComponent<Player>().isRunning = true;
        animator.GetComponent<PlayerAttack>().SetCanAttack(true);

        animator.GetComponent<Player>().components.ani.SetInteger("Attack", 0);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
