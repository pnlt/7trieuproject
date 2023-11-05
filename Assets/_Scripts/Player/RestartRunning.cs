using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartRunning : StateMachineBehaviour
{
    static int DeadHash = Animator.StringToHash("isDied");

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // We don't restart if we go toward the death state
        if (animator.GetInteger(DeadHash)==1)
            return;

        GameManager._instance.SetGamePause(false);
    }
}
