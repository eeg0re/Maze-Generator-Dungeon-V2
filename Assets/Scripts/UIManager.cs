using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverScreen; 
    public GameObject slime;
    private PlayerEnemyCollision playerEnemyScript;

    private void Awake(){
        GameOverScreen.SetActive(false);
        playerEnemyScript = slime.GetComponent<PlayerEnemyCollision>();
    }

    void Update(){
    }
}
