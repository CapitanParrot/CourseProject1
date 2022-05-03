using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

// Объект спавнер
// У него в детях должны быть точки, в которых будут спавнится враги.
public class EnemySpawner : MonoBehaviour
{
    private static System.Random rnd;

    // Спавн обычных врагов.
    public void Spawn(List<GameObject> enemies)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Instantiate(enemies[rnd.Next(enemies.Count)], transform.GetChild(i).transform.position, Quaternion.identity,
                GameManager.Instance.LevelCreatorInstance.transform);
        }
    }

    // Спавн босса.
    public void SpawnBoss()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Instantiate(EnemyManager.Instance.Bosses[GameManager.Instance.LevelCounter - 1],
                transform.GetChild(i).transform.position, Quaternion.identity,
                GameManager.Instance.LevelCreatorInstance.transform);
        }
    }
    
    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
    }
}
