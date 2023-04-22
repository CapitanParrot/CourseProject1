using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    public Grid Decor;
    public bool IsClear;
    public static Room CurrentRoom;
    public Grid walls;

    public bool IsFightRunning = false;
    public Grid InstanceWalls;

    public bool isBossRoom = false;

    //  огда игрок задевает тригер возле входа в еще не пройденной комнате, она закрываетс€ и начинаетс€ бой.
    void OnTriggerEnter2D(Collider2D other)
    {
        CurrentRoom = this;
        if (!IsClear && !IsFightRunning && other.tag.Equals("Player"))
        {
            if (!isBossRoom)
            {
                ((EnemySpawner) Decor.GetComponentInChildren(typeof(EnemySpawner))).Spawn(
                    EnemyManager.Instance.Enemies[GameManager.Instance.LevelCounter - 1]);
            }
            else
            {
                ((EnemySpawner)Decor.GetComponentInChildren(typeof(EnemySpawner))).SpawnBoss();
            }
            IsFightRunning = true;
            LockRoom();
        }
    }

    // «аканчивает бой.
    public void FinishFight()
    {
        IsFightRunning = false;
        IsClear = true;
        UnlockRoom();
        if (isBossRoom)
        {
            if (GameManager.Instance.LevelCounter < GameManager.Instance.EndLevel)
            {
                GameManager.Instance.Exit.gameObject.SetActive(true);
            }
            else
            {
                LockRoom();
            }
        }
    }


    private void LockRoom()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).tag.Equals("FightWall"))
            {
                transform.parent.GetChild(i).gameObject.SetActive(true);
            }
        }
        
    }

    private void UnlockRoom()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).tag.Equals("FightWall"))
            {
                transform.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

}
