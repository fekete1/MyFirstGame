using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy")){
            EnemyController enemy = collider.GetComponent<EnemyController>();

            if (enemy != null){
                enemy.DamageEnemy(damage);
            }
        }
    }
}
