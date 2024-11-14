using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public class Snake_Mechanics : MonoBehaviour
{
    private bool starting;
    private Transform snakehead_Transform;
    private Vector2 direction;
    private float speed;
    public BoxCollider2D food_spawn_region;
    private List<GameObject> SnakeBody;
    public GameObject snakeBody_prefab;

    private int Starting_Size;

    // Start is called before the first frame update
    void Start()
    {
        Starting_Size = 3;
        SnakeBody = new List<GameObject>();
        snakehead_Transform = this.transform;
        snakehead_Transform.position = new Vector2(0f,0f);
        SnakeBody.Add(this.gameObject);
        speed = 0.05f;
        Time.fixedDeltaTime = speed;
        start_growth();
    }
    void start_growth()
    {
        direction = Vector2.right * snakehead_Transform.lossyScale.x;
        for(int i = 0;i < Starting_Size; i++)
        {
            body_growth();
            snake_move();
        }
        direction = new Vector2(0f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        input_manager();
    }

    /* This Function Basically Handles the Game Functionality based on the Input of user */
    private void input_manager()
    {
        if(direction != Vector2.down * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up * snakehead_Transform.lossyScale.x;
        }
        else if(direction != Vector2.up * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down * snakehead_Transform.lossyScale.x;
        }
        else if(direction != Vector2.right * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left * snakehead_Transform.lossyScale.x;
        }
        else if(direction != Vector2.left * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right * snakehead_Transform.lossyScale.x;
        }
    }

    private void FixedUpdate()
    {
        snake_move();
    }
    private void snake_move()
    {
        if(direction != new Vector2(0f,0f))
        {
            for(int i = SnakeBody.Count - 1; i > 0; i--)
            {
                SnakeBody[i].transform.position = SnakeBody[i - 1].transform.position;
            }
            // move the snake based on the last known direction in fixed intervals
            snakehead_Transform.position = new Vector2(snakehead_Transform.position.x + direction.x,snakehead_Transform.position.y + direction.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if his by a wall
        if(collision.tag == "Wall")
        {
            snakehead_Transform.position = new Vector2(0f,0f);
            kill_snake();
        }
        // if the snake catches food
        if(collision.tag == "Food")
        {
            randomizefood(collision);
            body_growth();
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
    private void body_growth()
    {
        var temp = Instantiate(snakeBody_prefab);
        SnakeBody.Add(temp.gameObject);
        var n = SnakeBody.Count;
        SnakeBody[n-1].transform.position = SnakeBody[n-2].transform.position;
    }
    private void kill_snake()
    {
        for(int i = 1; i < SnakeBody.Count; i++)
        {
            Destroy(SnakeBody[i]);
        }
        SnakeBody = new List<GameObject>();
        SnakeBody.Add(this.gameObject);
        start_growth();
    }
}
