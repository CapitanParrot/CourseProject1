using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// Снаряд.
public class Fireball : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D RB;

    // Направление полета.
    public UnityEngine.Vector3 Direction;

    public int Damage;

    public void Fire()
    {
        RB.velocity = Direction * Speed;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            PlayerManager.Instance.TakeDamage(Damage);
            Destroy(gameObject);
        }
        if (other.transform.tag == "Wall" || other.transform.tag == "FightWall" || other.transform.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
