using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, GotDamaged, PlayerComponents,PlayerMovement,PlayerAttack
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D rb2d { get; set; }
    public Animator animator { get; set; }
    public BoxCollider2D bc2d { get; set; }
    [field: SerializeField] public LayerMask jumpableGround { get; set; }
    [field: SerializeField] public LayerMask climbableGround { get; set; }

    #region Player Animation
    public string playerIdle = "Player_Idle";
    public string playerRun = "Player_Running";
    public string playerJump = "Player_Jump";
    public string playerFall = "Player_Fall";
    public string playerDoubleJump = "Player_DoubleJump";
    public string playerWallHold = "Player_Climb";
    #endregion

    #region State Machine Variables
    public PlayerStateMachine stateMachine { get; set; }
    public HorizontalMove runState { get; set; }
    public JumpMove jumpState { get; set; }
    public WallHolding wallHoldState { get; set; }
    public NormalAttack attackState { get; set; }
    #endregion

    #region Movement (Left & Right)

    #region Variables
    [field: SerializeField] public float moveSpeed { get; set; } = 5f;
    public bool IsFacingRight { get; set; } = true;

    public float posX; public float posY; public Vector3 flip;
    #endregion

    #region Function
    public void movementInput(out float horizontal,out float vertical)
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    public void CheckForLeftOrRightFacing()
    {
        IsFacingRight = !IsFacingRight;

        flip = transform.localScale; flip.x = -1;

        if(flip.x > 0) { transform.Rotate(0, 0, 0); }
        else if(flip.x < 0) { transform.Rotate(0, 180, 0); }
    }
    #endregion

    #endregion

    #region Terrain Setup

    #region Variables
    [field: SerializeField] public float jumpForce { get; set; } = 14f;
    public short jumpStep { get; set; }
    public bool isHoldingWall { get; set; } = true;
    #endregion

    #region CheckTerrain
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    
    public bool IsClimbable(float minDistance, float maxDistance)
    {
        float rangeLimitDistance = Mathf.Clamp(maxDistance,minDistance,maxDistance);

        bool rightClimb = CheckDirection(Vector2.right, rangeLimitDistance);
        bool leftClimb = CheckDirection(Vector2.left, rangeLimitDistance);

        return rightClimb || leftClimb;
    }

    public bool CheckDirection(Vector2 direction, float maxDistance)
    {
        RaycastHit2D hit = Physics2D.Raycast(bc2d.bounds.center, direction, maxDistance, climbableGround);

        if(hit.collider != null)
        {
            return Mathf.Abs(hit.point.x - bc2d.bounds.center.x) <= maxDistance;
        }

        return false;
    }
    #endregion

    #endregion

    #region Attack Variables
    public float attackTime { get; set; }
    [field: SerializeField] public float attackDuration { get; set; } = 0.5f;
    [field: SerializeField] public float attackCD { get; set; } = 1f;
    public float attackCDtime { get; set; }
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        runState = new HorizontalMove(this, stateMachine);
        jumpState = new JumpMove(this, stateMachine);
        wallHoldState = new WallHolding(this, stateMachine);
        attackState = new NormalAttack(this, stateMachine);
    }
    private void Start()
    {
        CurrentHealth = MaxHealth;

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();

        flip.x = 1; jumpStep = 2; isHoldingWall = false;
        attackCDtime = attackCD;

        stateMachine.Initialize(runState);
    }
    private void FixedUpdate()
    {
        #region Flip Condition
        if (posX > 0 && !IsFacingRight)
        {
            CheckForLeftOrRightFacing();
        }

        else if(posX < 0 && IsFacingRight)
        {
            CheckForLeftOrRightFacing();
        }
        #endregion

        stateMachine.CurrentPlayerState.PhysicsUpdate();
    }
    private void Update()
    {
        #region Player Movement
        movementInput(out posX,out posY);
        rb2d.velocity = new Vector2(posX * moveSpeed, rb2d.velocity.y);
        #endregion

        #region Player Attack
        if (attackCDtime > 0f) attackCDtime -= Time.deltaTime;
        if (attackCDtime <= 0f) attackCDtime = 0f;
        #endregion

        stateMachine.CurrentPlayerState.FrameUpdate();
    }

    #region Health && Die

    #region Got Damage by Trap
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("deathTrap"))
        //{
        //    Die();
        //}

        //if (collision.gameObject.CompareTag("normalTrap"))
        //{
        //    CurrentHealth -= 20f;
        //}
    }
    #endregion

    #region Got Damage by Enemy
    #endregion
    public void Damage()
    {
        if (CurrentHealth <= 0f)
        {
            Die();

            RestartLevel();
        }
    }

    public void Die()
    {
        rb2d.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("death");
    }

    IEnumerator WaitingLevelRestart()
    {
        yield return new WaitForSeconds(10f);
    }

    public void RestartLevel()
    {
        StartCoroutine(WaitingLevelRestart());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion


}
