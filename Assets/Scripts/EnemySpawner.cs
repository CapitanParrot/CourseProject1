using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    //private GameObject gameManager;

    //private GameManager GM;
    // Start is called before the first frame update

    // Спавнит врагов на точки.
    private static System.Random rnd;
    public void Spawn()
    {
        var enemies = EnemyManager.Instance.Enemies;
        for (int i = 0; i < transform.childCount; i++)
        {
            Instantiate(enemies[rnd.Next(enemies.Count)], transform.GetChild(i).transform.position, Quaternion.identity);
        }
    }

    public void SpawnBoss()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Instantiate(EnemyManager.Instance.Bosses[GameManager.Instance.LevelCounter - 1], transform.GetChild(i).transform.position, Quaternion.identity);
        }
    }
    void Start()
    {
        //Instantiate(GameManager.Instance.Enemies[0], transform.position, Quaternion.identity);
    }
    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
        //gameManager = GameObject.FindWithTag("GameManager");
        //GM = gameManager.GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
