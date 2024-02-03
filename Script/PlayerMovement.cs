using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //GLOBAL FUNCTION
    public UpdateFlip flip;
    public SkillCasting skill;


    //PRIVATE FUNCTION
    private Rigidbody2D rb2d;

    public Animator animator;

    private BoxCollider2D bc2d;

    private SpriteRenderer spriteRenderer;

    public float posX, posY;
    [SerializeField] private float moveSpeed = 11f;
    [SerializeField] private float jumpForce = 14f;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask climbableTerrain;

    private int stepCount = 0;

    string[] keyLimit = new string[] { "Jump", "Dash", "Horizontal" };

    //MOVEMENT STATE VARIABLES
    public string currentState;
    const string Player_Idle = "Player_Idle";
    const string Player_Run = "Player_Running";
    const string Player_Jump = "Player_Jump";
    const string Player_Fall = "Player_Fall";
    const string Player_x2Jump = "Player_DoubleJump";
    const string Player_Dash = "Player_Dash";
    const string Player_wallHold = "Player_Climb";

    //SKILL STATE VARIABLES
    const string skill_DemonDance = "Player_Skill_DemonDance";
    private float durationDemonDance = 1.6f;

    [SerializeField]
    public bool isDDSkilled = false;
    public float DDTimer;
    public float DDCooldown = 7f;
    public float DDCooldownTimer;

    //DASH VARIABLES
    public Vector2 dashVelocity = new Vector2();

    [SerializeField]
    public float dashDistance = 20f;
    public float dashDuration = 0.5f;
    public bool isDashing = false;

    private float dashTimer;
    private float dashCooldown = 1f;
    private float dashCooldownTimer;

    public bool isWallHolding = false;

    [SerializeField] private AudioSource jumpSoundEffect;
    void Start()
    {
        Time.timeScale = 1.0f;
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        skill = GetComponent<SkillCasting>();
        flip = GetComponent<UpdateFlip>();

        stepCount = 2;

        dashCooldownTimer = dashCooldown;
        DDCooldownTimer = DDCooldown;
    }
    public void movementInput(out float horizontal, out float vertical)
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    void Update()
    {
        movementInput(out posX,out posY);

        skill = FindObjectOfType<SkillCasting>();

        //DASH CONDITION & MOVEMENT RESPONSE
        if (Input.GetButtonDown("Dash") && !isDashing && dashCooldownTimer <= 0f) { Dash(); }

        if (isDashing == true)
        {
            dashVelocity = transform.right * (dashDistance / dashDuration);
            rb2d.velocity = dashVelocity;
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb2d.velocity = Vector2.zero;
                dashCooldownTimer -= Time.deltaTime;
            }
        }
        else rb2d.velocity = new Vector2(posX * moveSpeed, rb2d.velocity.y);

        if (dashCooldownTimer > 0f) { dashCooldownTimer -= Time.deltaTime; }

        if (skill.skill[skill.checkpointIndex].skillCasted == false)
        {
            UpdateAnimation();
        }
    }
    private void UpdateAnimation()
    {
        //TEST BOX
        //Debug.Log(currentState);

        //DASH
        if (isDashing == true) { changeAnimationState(Player_Dash); }

        //MOVE LEFT & RIGHT
        if (IsGrounded() && !isDashing && !skill.DScasted && !skill.DDcasted && !isDDSkilled)
        {
            if (posX != 0) { changeAnimationState(Player_Run); }
            else
            {
                if(isWallHolding == false)
                    changeAnimationState(Player_Idle);
            }
            stepCount = 2;
        }

        //JUMP & DOUBLE JUMP
        Jump();

        if (rb2d.velocity.y > .1f && !IsGrounded())
        {
            if (stepCount == 2) { changeAnimationState(Player_Jump); }
            if (stepCount == 1) { changeAnimationState(Player_x2Jump); }
        }

        else if (rb2d.velocity.y < -.1f) { changeAnimationState(Player_Fall); }

        //WALL HOLD
        if (IsClimbable(0.3f, 0.5f) && Input.GetButtonDown("wallHold"))
        {
            if(!isWallHolding)
            {
                changeAnimationState(Player_wallHold);
                isDashing = false;
                rb2d.bodyType = RigidbodyType2D.Static;
                isWallHolding = true;
            }
        }
        else foreach (string keys in keyLimit)
        {
            if (Input.GetButtonDown(keys))
            {
                rb2d.bodyType = RigidbodyType2D.Dynamic;
                if (keys == "Jump") { IsGrounded(); Jump(); }
                if (keys == "Dash") { Dash(); }
                isWallHolding = false;
            }
        }

        //ATTACK
        //if (isAttacking == true) { changeAnimationState(Player_Attack); }

        //SKILL CAST
        if(isDDSkilled == true) { changeAnimationState(skill_DemonDance); }
    }
    public void DDCast()
    {
        if (!isDDSkilled && DDCooldownTimer < 0f)
        {
            isDDSkilled = true;
            DDTimer = durationDemonDance;
            DDCooldownTimer = DDCooldown;
        }
    }
    public void Dash()
    {
        if (!isDashing && dashCooldownTimer < 0f)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && stepCount > 1)
        {
            stepCount--;
            jumpSoundEffect.Play();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
    }

    //SkillCasting.Skill ClassOfSkill;
    public void changeAnimationState(string newState)
    {
        if (currentState == newState) return; 
        animator.Play(newState);
        currentState = newState;
    }
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    public bool IsClimbable(float minDistance, float maxDistance)
    {
        float rangeLimitDistance = Mathf.Clamp(maxDistance, minDistance, maxDistance);

        bool canClimbRight = CheckDirection(Vector2.right, rangeLimitDistance);
        bool canClimbLeft = CheckDirection(Vector2.left, rangeLimitDistance);

        return canClimbRight || canClimbLeft;
    }
    private bool CheckDirection(Vector2 direction, float maxDistance)
    {
        RaycastHit2D hit = Physics2D.Raycast(bc2d.bounds.center, direction, maxDistance, climbableTerrain);

        if (hit.collider != null)
        {
            return Mathf.Abs(hit.point.x - bc2d.bounds.center.x) <= maxDistance;
        }

        return false;
    }
}