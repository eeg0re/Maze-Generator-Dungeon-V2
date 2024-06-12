using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        GameObject slime = other.gameObject;
        if (slime.CompareTag("enemy")){
            //Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if(slime != null){
                Destroy(slime);
            }
        }
    }
}
