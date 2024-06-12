using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        GameObject playerr = other.gameObject;
        if (playerr.CompareTag("Player")){
            if(playerr != null){
                Debug.Log("GAME OVER");
            }
        }
    }
}
