using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMove : PlayerState
{
    public HorizontalMove(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        #region Main
        if (player.IsGrounded())
        {
            #region Run Animation
            if (player.posX != 0)
                player.animator.Play("Run");
            #endregion

            #region Idle Animation
            else if (player.posX == 0)
            {
                player.animator.Play("Idle");
            }
            #endregion

            player.jumpStep = 2;
        }
        #endregion

        #region Movement -> Jump
        if (Input.GetKeyDown(KeyCode.Space) && player.jumpStep >= 1)
            player.stateMachine.ChangeState(player.jumpState);
        #endregion

        #region Movement -> Attack
        if (Input.GetKeyDown(KeyCode.K) && player.attackCDtime <= 0f)
            player.stateMachine.ChangeState(player.attackState);
        #endregion
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
