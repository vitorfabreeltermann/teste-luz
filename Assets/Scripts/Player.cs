using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 200f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float radius = 0.2f;

    int extraJumps = 1;
    bool isJumping = false;
    bool isOnFloor = false;

    Rigidbody2D body;
    SpriteRenderer sprite;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //isOnFloor = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);

        //isOnFloor = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);

        isOnFloor = body.IsTouchingLayers(whatIsGround);

        //if (Input.GetButtonDown("Jump") && isOnFloor == true)
        //    isJumping = true;

        if  (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            isJumping = true;
            extraJumps--;
        }

        if (isOnFloor)
        {
            extraJumps = 1;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
        }

        PlayerAnimation();

    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(move * speed, body.velocity.y);

        if ((move > 0 && sprite.flipX == true) || (move < 0 && sprite.flipX == false))
        {
            Flip();
        }

        if (isJumping)
        {
            body.velocity = new Vector2(body.velocity.x, 0f);
            body.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }

        if (body.velocity.y > 0f && !Input.GetButton("Jump"))
        {
            body.velocity += Vector2.up * -0.8f;
        }
    }

    void Flip()
    {
        sprite.flipX = !sprite.flipX;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }

    void PlayerAnimation()
    {
        anim.SetFloat("VelX", Mathf.Abs(body.velocity.x));
        anim.SetFloat("VelY", Mathf.Abs(body.velocity.y));
    }
}
