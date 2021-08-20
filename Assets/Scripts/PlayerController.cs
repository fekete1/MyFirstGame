using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private GameController  _GameController;

    private Rigidbody2D     playerRb;
    private Animator        playerAnimator;
    private SpriteRenderer  playerSprite;

    public float             speed;
    public float             runSpeed;
    public float             jumpForce;
    public bool              isLookLeft;
    public bool              isRunning;

    public Transform         leftFeet;
    public Transform         rightFeet;
    private bool             isGrounded;

    private bool             isAtack;
    public Transform         hand;
    public GameObject        hitBox;
    public GameObject        shuriken;
    
    public float             shurikenRate;
    public float             nextShuriken;
    public int               amountShuriken;
    public int               shurikenSpeed;

    public int               amountBanana;

    public int               health = 3;
    public Color             onHitColor;
    public Color             onInvecibleColor;
    public bool              isAlive;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        _GameController.playerTransform = this.transform;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameController.isPause != true && isAlive == true){
            Move();
            Jump();
            Atack();
            Run();
            ShurikenThrow();
        }
      
    }

    void FixedUpdate()
    {   if (_GameController.isPause != true){
            GroundCheck();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {   
        if (collider.gameObject.tag == "Collectable"){
            amountBanana +=1;
            _GameController.BananaCount();
            Destroy(collider.gameObject);
        }
        else if(collider.gameObject.tag == "Damage" && isAlive){
            health -= 1;
            _GameController.HeartController();
            Debug.Log(("perdi vida!", health));
            if (health > 0){
                StartCoroutine("DamageEffect");
            }
            else{
                isAlive = false;
                Invoke("ReloadLevel", 3f);
            }
            
        }
    }

    void GroundCheck(){
        //Checa se ta pisando em algo, pé esquerdo e pé direito
        bool left = Physics2D.OverlapCircle(leftFeet.position, 0.01f);
        bool right = Physics2D.OverlapCircle(rightFeet.position, 0.01f);
        isGrounded = left | right;
    }

    void Move(){
        
        float h = Input.GetAxisRaw("Horizontal");
        float speedY = playerRb.velocity.y;


        playerRb.velocity = new Vector2(h * speed, speedY);

        if(h > 0 && isLookLeft){
            Flip();
        }
        else if (h < 0 && !isLookLeft){
            Flip();

        }
        playerAnimator.SetInteger("h", (int) h);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speedY", speedY);

    }

    void Run(){
            isRunning = (Input.GetKey(KeyCode.E));
            if (isRunning == true && isGrounded == true){

                float h = Input.GetAxisRaw("Horizontal");
                playerRb.velocity = new Vector2(h * speed * runSpeed, playerRb.velocity.y);
            }
        
    }

    void Jump(){
        if (Input.GetButtonDown("Jump") && (isGrounded == true  && isAtack == false))
        {
            playerRb.velocity = Vector2.up * jumpForce;

        }
    }

    void Flip(){
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void Atack(){
        if (Input.GetButtonDown("Fire1") && isAtack == false){
            isAtack = true;
            playerAnimator.SetTrigger("atack");
        }

        playerAnimator.SetBool("isAtack", isAtack);
    }

 

    void EndAtack(){
        isAtack = false;
    }

    void HitBox(){
        GameObject hitBoxTemp = Instantiate(hitBox, hand.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }

    void ShurikenThrow(){
        if (Input.GetButtonDown("Fire2") && amountShuriken > 0 && Time.time > nextShuriken){
            nextShuriken = Time.time + shurikenRate;
            GameObject shurikenTemp = Instantiate(shuriken, hand.position, transform.localRotation);
            amountShuriken -=1;
            _GameController.ShurikenCount();

            if (isLookLeft){
                shurikenTemp.GetComponent<Rigidbody2D>().AddForce(Vector3.left * shurikenSpeed);
            }
            else{
                shurikenTemp.GetComponent<Rigidbody2D>().AddForce(Vector3.right * shurikenSpeed);
            }
        }
    }

    IEnumerator DamageEffect(){

        this.gameObject.layer = LayerMask.NameToLayer("Invencible");
        playerSprite.color = onHitColor;
        yield return new WaitForSeconds(0.2f);
        playerSprite.color = onInvecibleColor;
        for (int i = 0; i < 5; i++)
        {
            playerSprite.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSprite.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        playerSprite.color = Color.white;
    }

    void ReloadLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
