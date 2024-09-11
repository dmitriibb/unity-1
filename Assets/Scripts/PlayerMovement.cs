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
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    } 

    private void Update()
    {
        float horizontalInput = Input.GetAxis(AXIS_HORIZONTAL);
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);


        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        anim.SetBool(ANIM_CONDITION_RUN, horizontalInput != 0);
        anim.SetBool(ANIM_CONDITION_GROUNDED, grounded);

    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger(TRIGGER_JUMP);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TAG_GROUND)
            grounded = true;
    }
}
