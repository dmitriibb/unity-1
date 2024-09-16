using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string AXIS_HORIZONTAL = "Horizontal";
    const string ANIM_CONDITION_RUN = "run";
    const string ANIM_CONDITION_GROUNDED = "grounded";
    const string TAG_GROUND = "Ground";
    const string TRIGGER_JUMP = "jump";

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
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

        // print($"OnWall - {OnWall()}");
        if (wallJumpCooldown > 0.2f)
        {   
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (OnWall() && !IsGrounded()) {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            } else
                body.gravityScale = initialGravityScale;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger(TRIGGER_JUMP);
        }
        else if (OnWall() && !IsGrounded())
        {
            if (horizontalInput == 0)
            {
                // just jump off the wall and fall down
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, initialGravityScale);
            
            wallJumpCooldown = 0;
        }
        
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
