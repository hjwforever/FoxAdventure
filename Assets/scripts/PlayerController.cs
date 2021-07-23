using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private Collider2D coll;
    public Collider2D disColl;
    private Animator anim;

    public float speed,jumpForce;
    public Transform groundCheck;
    public Transform cellingCheck;
    public LayerMask ground;
    public int Cherry,Gem;
    public AudioSource jumpAudio,hurtAudio,cherryAudio,GemAudio;
    public Text CherryNum,GemNum;

    public bool isGround, isJump;

    bool jumpPressed,iscrouching;
    [SerializeField] private bool isHurt;
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && jumpCount >0)
        {
            jumpPressed = true;
        }

        if (Input.GetKey("s") && !isJump)
        {
            iscrouching = true;
        }
        if (Input.GetKey("w") && !isJump)
        {
            iscrouching = false;
        }

        Faceto();
    }

    private void FixedUpdate()
    {
        //isGround = Physics2D.OverlapCircle(groundCheck.position,0.1f,ground);

        if(!isHurt)
        GroundMovement();

        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {
            Jump();
        }
        SwitchAnim();
    }

    //角色移动
    void GroundMovement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedircetion  = Input.GetAxisRaw("Horizontal");
        if (facedircetion > 0) facedircetion = 1;
        else if (facedircetion < 0) facedircetion = -1;
        else facedircetion = 0;
        
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if(horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
            anim.SetFloat("running", Mathf.Abs(horizontalMove));
        }
        if(facedircetion != 0)
        {
            transform.localScale = new Vector3(facedircetion, 1, 1);
        }

    }


    //角色面向（强制Scale.x为整数，若为小数会变成纸片人）
    void Faceto()
    {
        if(transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

    //角色跳跃
    void Jump()
    {
      //  anim.SetBool("idle", false);
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
            
        }
        if(jumpPressed && isGround)
        {
            iscrouching = false;
            anim.SetBool("jumping", true);
            jumpAudio.Play();
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
             jumpCount--;
            jumpPressed = false;        
        }
        else if(jumpPressed && jumpCount > 0 && !isGround)
        {
            iscrouching = false;
            anim.SetBool("jumping", true);
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }


    }

    //切换动画效果
    void SwitchAnim()
    {
        //anim.SetBool("idle", false);

        if(rb.velocity.y<0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
           
            anim.SetBool("beingHurt",true);
            anim.SetFloat("running", 0);
        if (Mathf.Abs(rb.velocity.x)<0.1f)
            {
            isHurt = false;
            anim.SetBool("beingHurt", false);
            }
        }
        else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            //anim.SetBool("idle", true);
        }


        //趴下
        if (!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
            }
            else
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }
        }
/*        else
        {
            anim.SetBool("crouching", true);
            disColl.enabled = false;
        }   */
        
    }

    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //樱桃
        if (collision.tag == "Cherry")
        {
            cherryAudio.Play();
            collision.GetComponent<Animator>().Play("isGot");
            //Destroy(collision.gameObject);
            //Cherry++;
            //CherryNum.text = Cherry.ToString();
        }

        //宝石
        if (collision.tag == "Gem")
        {
            GemAudio.Play();
            collision.GetComponent<Animator>().Play("isGot");
            //Destroy(collision.gameObject);
            //Gem++;
            //GemNum.text = Gem.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
           Invoke(nameof(Restart), 2f);
        }
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //碰到敌人
        
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                //消灭敌人
                enemy.JumpOn();

                //人物小跳
                isJump = true;
               // jumpAudio.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce/2);
            }
            else if(transform.position.x<collision.gameObject.transform.position.x)
            {
                isHurt = true;
                hurtAudio.Play();
                rb.velocity = new Vector2(-4, rb.velocity.y);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                isHurt = true;
                hurtAudio.Play();
                rb.velocity = new Vector2(4, rb.velocity.y);
            }
        }
    }

    //重新开始
    void Restart()
    {
      Destroy(coll.gameObject);
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);     
    }

    public void CherryCount()
    {
        //cherryAudio.Play();
        //数字变化
        Cherry++;
        CherryNum.text = Cherry.ToString();        
    }

    public void GemCount()
    {
        //数字变化
        Gem++;
        GemNum.text = Gem.ToString();
    }
}
