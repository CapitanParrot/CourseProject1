using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FightBlocker : MonoBehaviour
{
    public Grid walls;

    public static Grid InstanceWalls;

    private bool oneInstFlag = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (oneInstFlag)
        //{
            
        //    InstanceWalls = Instantiate(walls, transform.position, Quaternion.identity, transform);
        //    //instanceWalls.transform.GetChild(0).localPosition = transform.localPosition;
        //    //Destroy(Instantiate(walls, transform.position, Quaternion.identity, transform), 1f);
        //    oneInstFlag = false;
        //    //print("Destroy");
        //}

    }

    // Update is called once per frame
    void Update()
    {
    }
}
