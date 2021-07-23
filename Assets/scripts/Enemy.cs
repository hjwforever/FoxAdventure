using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D coll;
    protected AudioSource deathAudio;

   protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        deathAudio = GetComponent<AudioSource>();
    }

    void beforeDeath()
    {

        rb.bodyType = RigidbodyType2D.Static;//设置刚体为静态
        rb.velocity = new Vector2(0,0);//清除所有的力
    }

    public void Death()
    {
       
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        gameObject.tag = "Untagged";
        anim.SetTrigger("dead");
        //gameObject.GetComponent<Collider2D>().enabled = false;
        deathAudio.Play();
    }



}
