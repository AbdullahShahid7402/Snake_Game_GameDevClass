using System;
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

    public BoxCollider2D food_spawn_region;
    // Start is called before the first frame update
    void Start()
    {
        snakehead_Transform = this.transform;
        snakehead_Transform.position = new Vector2(0f,0f);
        speed = 0.05f;
        Time.fixedDeltaTime = speed;
    }

    // Update is called once per frame
    void Update()
    {
        input_manager();
    }

    /* This Function Basically Handles the Game Functionality based on the Input of user */
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
        // move the snake based on the last known direction in fixed intervals
        snakehead_Transform.position = new Vector2(snakehead_Transform.position.x + direction.x,snakehead_Transform.position.y + direction.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if his by a wall
        if(collision.tag == "Wall")
        {
            snakehead_Transform.position = new Vector2(0f,0f);
        }
        // if the snake catches food
        if(collision.tag == "Food")
        {
            randomizefood(collision);
        }
    }

    /* this function is responsible of what happens when a food item is collected by the snake head */
    private void randomizefood(Collider2D collision)
    {
        // Store min and max regions of food spawn area
        Vector2 min = food_spawn_region.bounds.min;
        Vector2 max = food_spawn_region.bounds.max;
        // this vector will be the new spawn point for the food item
        Vector2 new_pos;
        do
        {
            new_pos = new Vector2(UnityEngine.Random.Range(min.x, max.x),UnityEngine.Random.Range(min.y, max.y));
            new_pos.x = (float)(Math.Floor(new_pos.x));
            new_pos.y = (float)(Math.Floor(new_pos.y));
        } while(this.GetComponent<Collider2D>().bounds.Contains(new_pos));
        // apply the new position
        collision.gameObject.transform.position = new_pos;
    }
}
