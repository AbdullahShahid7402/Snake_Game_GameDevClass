using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    public GameObject Snake;
    private bool keep_spawning;
    public GameObject Obstacle_prefab;
    private List<GameObject> obstacle;
    public Collider2D spawner_area;
    private int spawnobjects;
    // Start is called before the first frame update
    void Start()
    {
        obstacle = new List<GameObject>();
        spawnobjects = 3;
        keep_spawning = true;
        Invoke("halt_spawning",3f);
        spawn();
    }
    private void halt_spawning()
    {
        keep_spawning = false;
    }
    private void spawn()
    {
        // Store min and max regions of obstacle spawn area
        Vector2 min = spawner_area.bounds.min;
        Vector2 max = spawner_area.bounds.max;
        // this vector will be the new spawn point for the obstacle
        Vector2 pos;
        do
        {
            pos = new Vector2(UnityEngine.Random.Range(min.x, max.x),UnityEngine.Random.Range(min.y, max.y));
            pos.x = (float)(Math.Floor(pos.x));
            pos.y = (float)(Math.Floor(pos.y));
        } while(Snake.GetComponent<Collider2D>().bounds.Contains(pos));
        obstacle.Add(Instantiate(Obstacle_prefab));
        obstacle[0].transform.position = pos;
        while(keep_spawning)
        {
            if(obstacle.Count == spawnobjects)
            {
                break;
            }
            do
            {
                pos = new Vector2(UnityEngine.Random.Range(min.x, max.x),UnityEngine.Random.Range(min.y, max.y));
                pos.x = (float)(Math.Floor(pos.x));
                pos.y = (float)(Math.Floor(pos.y));
            } while(Snake.GetComponent<Collider2D>().bounds.Contains(pos));
            bool dist = true;
            for(int i = 0; i < obstacle.Count; i++)
            {
                if(Vector2.Distance(pos,obstacle[i].transform.position)<3f)
                {
                    dist = false;
                }
            }
            if(dist)
            {
                var temp = Instantiate(Obstacle_prefab);
                temp.transform.position = pos;
                obstacle.Add(temp);
            }
        }
    }
}