using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAnimationState : StateMachineBehaviour
{
    [SerializeField]
    private string componentName = "";

    [SerializeField]
    private bool printOnStateEnter;
    [SerializeField]
    private bool printOnStateUpdate;
    [SerializeField]
    private bool printOnStateExit;
    [SerializeField]
    private bool printOnAnimatorMove;
    [SerializeField]
    private bool printOnAnimatorIK;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (printOnStateEnter)
        {
            Debug.Log("AnimationState " + componentName + ": OnStateEnter (l:" + stateInfo.length + ", loop: " + stateInfo.loop + ")");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (printOnStateEnter)
        {
            Debug.Log("AnimationState " + componentName + ": OnStateUpdate (l:" + stateInfo.length + ", loop: " + stateInfo.loop + ")");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (printOnStateEnter)
        {
            Debug.Log("AnimationState " + componentName + ": OnStateExit (l:" + stateInfo.length + ", loop: " + stateInfo.loop + ")");
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion            
        if (printOnStateEnter)
        {
            Debug.Log("AnimationState " + componentName + ": OnStateMove (l:" + stateInfo.length + ", loop: " + stateInfo.loop + ")");
        }
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
        if (printOnStateEnter)
        {
            Debug.Log("AnimationState " + componentName + ": OnStateIK (l:" + stateInfo.length + ", loop: " + stateInfo.loop + ")");
        }
    }

}
