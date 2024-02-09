using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerMovement
{
    #region Horizontal Movement
    bool IsFacingRight { get; set; }
    float moveSpeed { get; set; } 
    void movementInput(out float horizontal, out float vertical);
    void CheckForLeftOrRightFacing();
    #endregion

    #region Vertical Movement
    float jumpForce { get; set; }
    short jumpStep { get; set; }
    bool isHoldingWall { get; set; }
    #endregion
}
