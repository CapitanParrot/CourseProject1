using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Chest : MonoBehaviour
{

    public Animator Animator;

    private bool isChestOpen = false;

    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            if (!isChestOpen)
            {
                isChestOpen = true;
                Animator.SetTrigger("OpenChest");
                GenerateTreasure();
                Destroy(gameObject,5);
            }
        }

    }

    public void GenerateTreasure()
    {
        Instantiate(ArtifactManager.Instance.GetArtifact(), transform.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
