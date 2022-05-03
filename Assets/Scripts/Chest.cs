using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

// Сундук спавнит сокровища, когда его откроют.
public class Chest : MonoBehaviour
{
    public Animator Animator;

    private bool isChestOpen = false;

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

    // Спавним сокровище.
    public void GenerateTreasure()
    {
        Instantiate(ArtifactManager.Instance.GetArtifact(), transform.position, Quaternion.identity,
            GameManager.Instance.LevelCreatorInstance.transform);
    }
}
