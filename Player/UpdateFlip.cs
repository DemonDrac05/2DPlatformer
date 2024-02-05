using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateFlip : MonoBehaviour
{
    private PlayerMovement player;

    public bool flipped = true;

    public Vector3 flip;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        flip.x = 1;
    }
    private void FixedUpdate()
    {
        if (player.posX > 0 && !flipped) { Flip(); }

        else if(player.posX < 0 && flipped) { Flip(); }
    }

    void Flip()
    {
        flipped = !flipped;

        flip = transform.localScale;
        flip.x *= -1;

        if(flip.x > 0) { transform.Rotate(0,0,0); }
        else if(flip.x < 0) { transform.Rotate(0,180,0); }
    }
}
