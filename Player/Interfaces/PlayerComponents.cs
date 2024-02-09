using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerComponents
{
    Rigidbody2D rb2d { get; set; }
    Animator animator { get; set; }
    BoxCollider2D bc2d { get; set; }
    LayerMask jumpableGround { get; set; }
    LayerMask climbableGround { get; set; }
    public bool IsGrounded();
    public bool IsClimbable(float minDistance, float maxDistance);
    public bool CheckDirection(Vector2 direction, float maxDistance);
}
