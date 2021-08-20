using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private GameController          _GameController;
    private Rigidbody2D             enemyRb;
    private Animator                enemyAnimator; 
    private Transform               player;
    private SpriteRenderer          enemySprite;

    public  int                     health;
    public float                    speed;
    public int                      h; // h = 0 parado, h = 1 andando
    public bool                     isMoving;
    public float                    distance;
    public Transform                groundCheck;

    public bool                     isLookLeft;


    void Awake()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;

        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find ("Player").GetComponent<Transform>();
        isMoving = true;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LimitWall" | collision.gameObject.tag == "Ground"){
            Flip();
        }
       
    }
    public void PatrolMove(){
        if (isMoving){
            h = 1;
        }
        else{
            h=0;
        }
        enemyRb.velocity = new Vector2(h * speed, enemyRb.velocity.y);
        RaycastHit2D ground = Physics2D.Raycast(groundCheck.position, Vector2.down, distance);

        if (ground.collider == false){
            Flip();
        }
        enemyAnimator.SetInteger("h",  h);
    }

    public void Flip(){
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        speed = -speed;
    }

    public void DamageEnemy(int damage){
        health -= damage;
        Debug.Log(health);
        StartCoroutine (DamageEffect());

        if (health <= 0){
            Destroy(this.gameObject);
        } 
    }

    IEnumerator DamageEffect(){
        enemySprite.color = Color.red;
        isMoving = false;
        yield return new WaitForSeconds(0.2f);
        enemySprite.color = Color.white;
        isMoving = true;
    }
}
