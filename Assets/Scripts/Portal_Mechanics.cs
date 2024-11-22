// i210721

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Mechanics : MonoBehaviour
{
    public GameObject Snake;
    public GameObject Portal_Prefab;
    public BoxCollider2D portal_spawn_region;
    private GameObject[] Portal;
    // Start is called before the first frame update
    void Start()
    {
        Portal = new GameObject[2];
        Portal[0] = Instantiate(Portal_Prefab);
        Portal[1] = Instantiate(Portal_Prefab);
        Spawn_Portals();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Spawn_Portals()
    {
        // Store min and max regions of portal spawn area
        Vector2 min = portal_spawn_region.bounds.min;
        Vector2 max = portal_spawn_region.bounds.max;
        // this vector will be the new spawn point for the portal
        Vector2 new_pos;
        do
        {
            new_pos = new Vector2(UnityEngine.Random.Range(min.x, max.x),UnityEngine.Random.Range(min.y, max.y));
            new_pos.x = (float)(Math.Floor(new_pos.x));
            new_pos.y = (float)(Math.Floor(new_pos.y));
        } while(Snake.GetComponent<Collider2D>().bounds.Contains(new_pos));
        Portal[0].transform.position = new_pos;
        do
        {
            new_pos = new Vector2(UnityEngine.Random.Range(min.x, max.x),UnityEngine.Random.Range(min.y, max.y));
            new_pos.x = (float)(Math.Floor(new_pos.x));
            new_pos.y = (float)(Math.Floor(new_pos.y));
        } while(Snake.GetComponent<Collider2D>().bounds.Contains(new_pos) && Vector2.Distance(Portal[0].transform.position,new_pos) < 7.25f);
        Portal[1].transform.position = new_pos;

        Invoke("Spawn_Portals",10f);
    }
    public Vector2 Teleport(Vector2 current_Portal)
    {
        if(current_Portal == new Vector2(Portal[0].transform.position.x,Portal[0].transform.position.y))
        {
            return Portal[1].transform.position;
        }
        return Portal[0].transform.position;
    }
}
