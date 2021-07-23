using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    //private Rigidbody2D rb;
    //private Animator anim;
    //private Collider2D coll;

    public LayerMask Ground;
    public Transform leftpoint;
    public Transform rightpoint;

    public float speed, jumpforce;
    private float leftx, rightx;

    private bool FaceLeft = true;

    protected override void Start()
    {
        base.Start();

        //rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        //coll = GetComponent<Collider2D>();

        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;

        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }


    void Update()
    {
        SwitchAnim();

    }

    void Movement()
    {
        if (FaceLeft&&!anim.GetBool("dead"))
        {
            if (transform.position.x < leftx + speed * 2 / 3)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                FaceLeft = false;
            }

            if (coll.IsTouchingLayers(Ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-transform.localScale.x * speed, jumpforce);
            }
        }
        else
        {
            if (transform.position.x > rightx - speed*2/3)
            {
                transform.localScale = new Vector3(1, 1, 1);
                FaceLeft = true;
            }
            if (coll.IsTouchingLayers(Ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-transform.localScale.x * speed, jumpforce);
            }

        }
        
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (coll.IsTouchingLayers(Ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
            rb.velocity = new Vector2(0, 0);
        }
    }
}
