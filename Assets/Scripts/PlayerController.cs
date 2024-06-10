using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates {
        IDLE,
        WALK,
        ATTACK
    }

    PlayerStates CurrentState{
        set{
            currentState = value;
            switch(currentState){
                case PlayerStates.IDLE:
                    animator.Play("Idle");
                    break;
                case PlayerStates.WALK:
                    animator.Play("Walk");
                    break;
                case PlayerStates.ATTACK:
                    animator.Play("Attack");
                    break;
            }
        }
    }

    public float moveSpeed = 1f; 

    Vector2 moveInput = Vector2.zero;
    Rigidbody2D rb; 
    Animator animator;
    SpriteRenderer spriteRenderer;

    PlayerStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // set the player's spawn point in the center of the maze
        this.transform.position = new Vector3(0, 0, -1);

    }

    private void FixedUpdate(){
        rb.velocity = moveInput * moveSpeed;
    }

    void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();
        if(moveInput != Vector2.zero){
            CurrentState = PlayerStates.WALK;

            animator.SetFloat("Xmove", moveInput.x);
            animator.SetFloat("Ymove", moveInput.y);
        }
        else{
            CurrentState = PlayerStates.IDLE;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
