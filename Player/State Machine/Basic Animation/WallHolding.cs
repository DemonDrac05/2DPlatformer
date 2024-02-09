using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHolding : PlayerState
{
    public WallHolding(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
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
        if (Input.GetButtonDown("wallHold") && player.IsClimbable(0.3f,0.5f) && !player.isHoldingWall)
        {
            player.animator.Play(player.playerWallHold);
            player.rb2d.bodyType = RigidbodyType2D.Static;
            player.isHoldingWall = true;
        }

        else if (Input.GetButtonDown("Jump"))
        {
            player.rb2d.bodyType = RigidbodyType2D.Dynamic;
            player.isHoldingWall = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
