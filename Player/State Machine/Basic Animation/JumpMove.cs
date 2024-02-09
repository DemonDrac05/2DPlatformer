using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpMove : PlayerState
{
    public JumpMove(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
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
        if (player.jumpStep >= 1)
        {
            player.jumpStep--;
            player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, player.jumpForce);

            #region Jump Animation
            if (player.rb2d.velocity.y > .1f && !player.IsGrounded())
            {
                if (player.jumpStep == 1)
                    player.animator.Play("Jump");
                if (player.jumpStep == 0)
                    player.animator.Play("Jump");
            }
            #endregion

            #region Fall Animation
            else if (player.rb2d.velocity.y < .1f)
                player.animator.Play(player.playerFall);
            #endregion
        }
        #endregion

        #region Jump -> Movement
        if (player.IsGrounded()) 
            player.stateMachine.ChangeState(player.runState);
        #endregion

        #region Jump -> Attack
        if (Input.GetKeyDown(KeyCode.K) && player.attackCDtime <= 0f)
            player.stateMachine.ChangeState(player.attackState);
        #endregion
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
