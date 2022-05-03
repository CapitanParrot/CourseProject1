using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Характеристики всех врагов.
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy", order = 2)]
public class EnemyStats : ScriptableObject
{
    public int Health;
    public int Damage;
    public List<GameObject> DropObjects;
    public int DropChance;
    public int Speed;
}
