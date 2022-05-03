using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    // Маленький скрипт для спавн поинт игрока.
    void Start()
    {
        GameManager.Instance.PlayerInstance.transform.position = transform.position;
    }
}
