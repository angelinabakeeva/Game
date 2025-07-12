using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private enum State { idle, run, jump, falling };
    private State state = State.idle;

    private Collider2D col;

    [SerializeField] private LayerMask Ground;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float rayDistance = 0.6f;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection > 0)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else if (hDirection < 0)
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }


        Vector2 rayStart = new Vector2(col.bounds.center.x, col.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, rayDistance, Ground);
        isGrounded = hit.collider != null;


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            state = State.jump;
        }

        VelocityState();
        anim.SetInteger("State", (int)state);
    }

    private void VelocityState()
    {
        if (state == State.jump)
        {
            if (rb.linearVelocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (isGrounded)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.linearVelocity.x) > 2f)
        {
            state = State.run;
        }
        else
        {
            state = State.idle;
        }
    }
}
