using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public int jumpForce;
    public int health;
    public Transform groudCheck;

    private bool ivunarable = false;
    private bool grounded = false;
    private bool jumping = false;
    private bool facingRight = true;
    private bool life = true;
    private int direction = 0;

    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Transform trans;

    private Camera cameraScript;

    public AudioClip fxHurt;
    public AudioClip fxJump;
    public AudioClip fxAttack;
    public AudioClip fxCoin;

    //variaveis para fazer o ataque
    public float attackRate;
    public Transform spawnAttack;
    public GameObject attackPrefab;
    public GameObject death;
    private float nextAttack = 0f;

    private Vector2 touchOrigin = -Vector2.one;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        //rd2d = GameObject.Find("Wolf").GetComponent<Rigidbody2D>(); // pega de outro personagem
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    
    void Update()
    {

        grounded = Physics2D.Linecast(trans.position,groudCheck.position, 1 << LayerMask.NameToLayer("Ground"));//cria uma linha entre o Player e a layer, visando verificar se o personagem esta tocando-a
        if(Input.GetButtonDown ("Jump") && grounded)
        {
            SoundManager.instance.PlaySound(fxJump);
            jumping = true;
        }
        SetAnimations();
        if(Input.GetButton("Fire1") && grounded && Time.time > nextAttack)
        {
            
            Attack();
        }
        ControlTouch();
        float move = Input.GetAxis("Horizontal");
        if (move != 0)
        {
            ControlTouch(move);
        }


    }
    private void FixedUpdate()
    {
        
        
    }

        void Flip()
    {
        facingRight = !facingRight;
        trans.localScale = new Vector3(-trans.localScale.x, trans.localScale.y, trans.localScale.z);
    }
    void SetAnimations()
    {
        anim.SetFloat("VelY", rb2d.velocity.y);
        anim.SetBool("JumpFall", !grounded);
        anim.SetBool("Walk", rb2d.velocity.x != 0f && grounded);
    }

    void Attack()
    {
        anim.SetTrigger("Punch");
        nextAttack = Time.time + attackRate;

        GameObject cloneAttack = Instantiate(attackPrefab, spawnAttack.position, spawnAttack.rotation);
        SoundManager.instance.PlaySound(fxAttack);
        if (!facingRight)
        {
            cloneAttack.transform.eulerAngles = new Vector3 (180, 0, 180);
        }
    }
    IEnumerator DamageEffect()
    {
        for (float i =0f;i < 1f; i += 0.1f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        ivunarable = false;
    }
    public void DamagePlayerWater()
    {
        PlayerDeath();
        Debug.Log("Morreu");
        Invoke("ReloadLevel", 3f);
        gameObject.SetActive(false);
    }
    public void DamagePlayer()
    {
        if (!ivunarable)
        {

            ivunarable = true;
            health--;
            StartCoroutine(DamageEffect());
            if (health < 1)
            {
                PlayerDeath();
                Debug.Log("Morreu");
                // Destroy(gameObject);
                Invoke("ReloadLevel", 3f);
                gameObject.SetActive(false);

                
            }

        }
    }
    void PlayerDeath()
    {
        GameObject cloneDeath = Instantiate(death, transform.position, Quaternion.identity);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        gameObject.SetActive(true);
    }

    public void Rigth()
    {
        direction = 1;
    }

    public void Left()
    {
        direction = -1;
    }

    public void Stopped()
    {
        direction = 0;
    }
    public void Jump()
    {
        if (grounded)
        {
            SoundManager.instance.PlaySound(fxJump);
            jumping = true;
        }
    }
    
    private void ControlTouch()
    {
        Debug.Log(direction);
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
        //transform.Translate(direction * Time.deltaTime, 0, 0);
        if ((direction < 0 && facingRight) || (direction > 0f && !facingRight))
        {
            Flip();
        }
        if (jumping)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
    }
    private void ControlTouch(float direction)
    {
        Debug.Log(direction);
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
        //transform.Translate(direction * Time.deltaTime, 0, 0);
        if ((direction < 0 && facingRight) || (direction > 0f && !facingRight))
        {
            Flip();
        }
        if (jumping)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
    }  
}
