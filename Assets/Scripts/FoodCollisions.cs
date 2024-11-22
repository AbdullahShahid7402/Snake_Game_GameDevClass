using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodCollisions : MonoBehaviour
{
    public BoxCollider2D food_spawn_region;
    public GameObject Snake;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /* this function is responsible of what happens when a food item is collected by the snake head */
    public void randomizefood()
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
        } while(Snake.GetComponent<Collider2D>().bounds.Contains(new_pos));
        // apply the new position
        this.gameObject.transform.position = new_pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall" || collision.tag == "Portal" || collision.tag == "Snake Head")
        {
            randomizefood();
        }
    }
}
