using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    public bool                 isRotation = true;
    public int                  damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
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

    void Rotation(){
        if (isRotation == true){
             transform.Rotate(new Vector3(x: 0, y: 0, z: 5));
        }
    }

    void StopRotation(){
        isRotation = false;
    }
}
