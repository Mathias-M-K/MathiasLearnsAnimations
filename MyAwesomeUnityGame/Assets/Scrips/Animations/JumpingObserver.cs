using System;
using UnityEditor.PackageManager;
using UnityEngine;


namespace Animations
{
    public class JumpingObserver : StateMachineBehaviour
    {
        public CharacterController cc;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Nothing
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (cc == null)
            {
                Debug.LogError("Character Controller not set");
            }
            else
            {
                cc.SetAnimationJumpState(false);
            }
        }

        public void SetCharacterController(CharacterController newCc)
        {
            cc = newCc;
        }
    }
}
