using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string AXIS_HORIZONTAL = "Horizontal";
    const string ANIM_CONDITION_RUN = "run";
    const string ANIM_CONDITION_GROUNDED = "grounded";
    const string TAG_GROUND = "Ground";
    const string TRIGGER_JUMP = "jump";

    [Header("Movements")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    // private bool grounded;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float initialGravityScale;
    private float horizontalInput;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        initialGravityScale = body.gravityScale;
        Utils.SetPlayer(gameObject);
    } 

    private void Update()
    {
        horizontalInput = Input.GetAxis(AXIS_HORIZONTAL);    

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool(ANIM_CONDITION_RUN, horizontalInput != 0);
        anim.SetBool(ANIM_CONDITION_GROUNDED, IsGrounded());

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0) {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }

        if (OnWall()) {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        } else {
            body.gravityScale = initialGravityScale;
            body.velocity = new (horizontalInput * speed, body.velocity.y);

            if (IsGrounded()) {
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            } else {
                coyoteCounter -= Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !OnWall() && jumpCounter <= 0) return;

        if (OnWall()) {
            jumpCounter = extraJumps;
            WallJump();
        } else {
            if (IsGrounded())
                body.velocity = new (body.velocity.x, jumpPower);
            else if (coyoteCounter > 0) {
                body.velocity = new (body.velocity.x, jumpPower);
                coyoteCounter = 0;
            } else if (jumpCounter > 0) {
                body.velocity = new (body.velocity.x, jumpPower);
                jumpCounter--;
            }

        }
    }

    private void WallJump()
    {
        
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == TAG_GROUND)
    //         grounded = true;
    // }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
}
