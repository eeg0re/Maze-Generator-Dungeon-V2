using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f; 
    public Vector2 offset;
    private Vector2 velocity = Vector2.zero;

    public Vector2 minPos;
    public Vector2 maxPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){
            Vector2 targetPos = ( new Vector2(target.position.x, target.position.y)) + offset;
            Vector2 xyPos = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            xyPos.x = Mathf.Clamp(xyPos.x, minPos.x, maxPos.x);
            xyPos.y = Mathf.Clamp(xyPos.y, minPos.y, maxPos.y);

            transform.position = new Vector3(xyPos.x, xyPos.y, -50);

        }
        
    }
}
