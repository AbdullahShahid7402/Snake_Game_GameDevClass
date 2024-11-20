using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Mechanics : MonoBehaviour
{
    public GameObject Portal_Prefab;
    public BoxCollider2D portal_spawn_region;
    private GameObject[] Portal;
    // Start is called before the first frame update
    void Start()
    {
        Portal = new GameObject[2];
        Portal[0] = Instantiate(Portal_Prefab);
        Portal[1] = Instantiate(Portal_Prefab);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Spawn_Portals()
    {

    }
}
