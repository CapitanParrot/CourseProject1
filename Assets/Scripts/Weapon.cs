using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public int Damage;
    public float Knockback;
    public float AttackSpeed;
    public float Scale;
    public int EnemiesPerHit;
    public float Accuracy;
}
