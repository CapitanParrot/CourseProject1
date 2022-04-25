using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public static Vector2[] Directions = new[] { Vector2.down, Vector2.left, Vector2.right, Vector2.up };
    void TakeDamage(int damage, Transform attacker, float knockback);
    void Death();
    void Move();
    void Attack();

    void Drop();
}
