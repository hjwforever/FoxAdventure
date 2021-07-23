using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    //private Rigidbody2D rb;

    public Transform uppoint;
    public Transform doenpoint;

    public float speed;
    private float upy, downy;

    private bool Flyhigher = true;
    protected override void Start()
    {
        base.Start();

        //rb = GetComponent<Rigidbody2D>();

        upy = uppoint.position.y;
        downy = doenpoint.position.y;

        Destroy(uppoint.gameObject);
        Destroy(doenpoint.gameObject);
    }


    void Update()
    {
        Movement();

    }

    void Movement()
    {
        if(Flyhigher)
        {
            if (transform.position.y > upy)
            {
                Flyhigher = false;
            }
            rb.velocity = new Vector2(0, speed);

        }
        else
        {
            if (transform.position.y < downy)
            {
                Flyhigher = true;
            }
            rb.velocity = new Vector2(0, -speed);
        }

    }
}
