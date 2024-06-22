using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private AudioSource jumpingSoundEffect;
    [SerializeField] private ParticleSystem dustParticle;

    [Header("Move Info")]
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float dirX_Speed = 7f;

    private bool canMove = true;

    private bool canDoubleJump = true;
    private bool canWallSlide;
    private bool isWallSliding;

    private bool facingRight = true;

    private float movingInput = 0f;

    private int facingDirection = 1;

    // Lực nhảy và hướng nhảy (phụ thuộc vào facingDirection)
    [SerializeField] private Vector2 wallJumpDirection = new Vector2(2f, 10f);

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    private bool isGrounded;
    private bool isWallDetected;

    //Them
    private bool isAppearing = true;

    // Method
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // Them
        StartCoroutine(DelayAppear());
    }

    //Them
    private IEnumerator DelayAppear()
    {
        anim.SetBool("isAppearing", isAppearing);
        yield return new WaitForSeconds(0.5f);
        isAppearing = false;
        anim.SetBool("isAppearing", isAppearing);
    }

    private void Update()
    {
        // Debug.Log("FacingRight: " + facingRight + " - FacingDirection: " + facingDirection);
        CollisionCheck();
        FlipController();
        ParticleSystemController();
        AnimatorController();
        CheckInput();
        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.2f);
            //Debug.Log("rb.velocity.y = " + rb.velocity.y);
        }

        Move();
    }

    private void CheckInput()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        // Hủy WallSlide khi nhấn "xuống"
        if (Input.GetAxisRaw("Vertical") < -0.1f)
            canWallSlide = false;
        if (Input.GetButtonDown("Jump"))
            JumpButton();
    }

    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movingInput * dirX_Speed, rb.velocity.y);
        }
    }
    private void ParticleSystemController()
    {
        if (movingInput != 0f && isGrounded)
            dustParticle.Play();
        
    }
    private void JumpButton()
    {
        if (isWallSliding)
        {
            WallJump();
        }
        else if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            Jump();
        }
        canWallSlide = false;
    }

    private void Jump()
    {
        jumpingSoundEffect.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void WallJump()
    {
        canMove = false;
        canDoubleJump = true;
        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
    }

    // Checked
    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    // Checked
    private void FlipController()
    {
        if (rb.velocity.x > 0.1f && !facingRight)
            Flip();
        else if (rb.velocity.x < -0.1f && facingRight)
            Flip();
    }

    private void AnimatorController()
    {
        bool isMoving = (movingInput != 0f);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        if (isWallDetected && rb.velocity.y < -0.1f)
            canWallSlide = true;
        if (!isWallDetected)
        {
            canWallSlide = false;
            isWallSliding = false;
        }
    }

    // Checked
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}