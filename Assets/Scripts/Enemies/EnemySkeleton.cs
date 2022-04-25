using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySkeleton : MonoBehaviour, IEnemy
{
    public EnemyStats Stats;
    private int currentHealth;

    public Rigidbody2D RB;
    //public float Speed;

    public Vector2 Direction;

    //public int Damage = 1;

    //public List<GameObject> DropObjects;
    //public int DropChance = 25;
    private Random rnd;

    //private System.Random rnd = new System.Random();

    //public float MoveTime = 1;

    //public float moveTimeCounter = 0;

    //public int Wait = 3;

    //private bool waitFlag = true;

    public float EnemyKnockback = 10;

    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
    }
    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
        print("EnemySkeleton destroyed");
        Destroy(gameObject);
        Drop();
    }

    public void Move()
    {
        RB.AddForce(Direction * Stats.Speed);
    }

    public void TakeDamage(int damage, Transform attacker, float knockback)
    {
        Push(transform.position - attacker.position, knockback);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Push(Vector2 direction, float strength)
    {
        RB.AddForce(direction.normalized * strength, ForceMode2D.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.Instance.AddEnemy();
        Direction = IEnemy.Directions[rnd.Next(IEnemy.Directions.Length)];
        ChangeSpriteSide();
        currentHealth = Stats.Health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        //if (Direction == Vector2.up || Direction == Vector2.right)
        //{
        //    transform.localScale = new Vector3(1f, 1f, 1f);
        //}
        //else
        //{
        //    transform.localScale = new Vector3(-1f, 1f, 1f);
        //}
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Wall" || other.transform.tag == "FightWall")
        {
            
        }
        if (other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
        Direction = -Direction;
        ChangeSpriteSide();
    }

    private void ChangeSpriteSide()
    {
        if (Direction == Vector2.up || Direction == Vector2.right)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void Drop()
    {
        if (rnd.Next(100) < Stats.DropChance)
        {
            Instantiate(Stats.DropObjects[rnd.Next(Stats.DropObjects.Count)], transform.position, Quaternion.identity);
        }
    }
}
