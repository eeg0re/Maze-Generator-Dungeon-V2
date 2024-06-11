using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private string[] directions = {"up", "down", "left", "right"};

    private string targetDirection;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        targetDirection = directions[Random.Range(0, directions.Length)];
    }

    private void FixedUpdate(){
        UpdateTargetDirection();
        SetVelocity();
    }

    private void UpdateTargetDirection(){
        targetDirection = directions[Random.Range(0, directions.Length)];
    }

    private void SetVelocity(){
        if(targetDirection == "up"){
            rb.velocity = transform.up * speed;
        }
        else if(targetDirection == "down"){
            rb.velocity = -transform.up * speed;
        }
        else if(targetDirection == "right"){
            rb.velocity = transform.right * speed;
        }
        else{
            rb.velocity = -transform.right * speed;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
