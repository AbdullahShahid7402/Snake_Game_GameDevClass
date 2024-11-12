using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public class Snake_Mechanics : MonoBehaviour
{
    private Transform snakehead_Transform;
    private Vector2 direction;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        snakehead_Transform = this.transform;
        snakehead_Transform.position = new Vector2(0f,0f);
        speed = 0.1f;
        Time.fixedDeltaTime = speed;
    }

    // Update is called once per frame
    void Update()
    {
        input_manager();
    }

    private void input_manager()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up * snakehead_Transform.lossyScale.x;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down * snakehead_Transform.lossyScale.x;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left * snakehead_Transform.lossyScale.x;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right * snakehead_Transform.lossyScale.x;
        }
    }

    private void FixedUpdate()
    {
        snakehead_Transform.position = new Vector2(snakehead_Transform.position.x + direction.x,snakehead_Transform.position.y + direction.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            snakehead_Transform.position = new Vector2(0f,0f);
        }
    }
}
