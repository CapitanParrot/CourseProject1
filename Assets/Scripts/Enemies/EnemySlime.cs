using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemySlime : MonoBehaviour, IEnemy
{
    public EnemyStats Stats;
    private int currentHealth;

    public Rigidbody2D RB;
    //public float Speed;

    public Vector2 Direction;

    //public List<GameObject> DropObjects;
    //public int DropChance = 30;
    private System.Random rnd;
    //public int Damage = 1;
    //private System.Random rnd = new System.Random();

    public float MoveTime = 1;

    public float moveTimeCounter = 0;

    public int Wait = 3;

    private bool waitFlag = true;

    public float EnemyKnockback = 10;

    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
    }

    void Start()
    {
        Direction = IEnemy.Directions[rnd.Next(4)];
        EnemyManager.Instance.AddEnemy();
        currentHealth = Stats.Health;
    }

    public void TakeDamage(int dmg, Transform attacker, float knockback)
    {
        Push(transform.position - attacker.position, knockback);
        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    private void Push(Vector2 direction, float strength)
    {
        RB.AddForce(direction.normalized * strength, ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Wall" || other.transform.tag == "FightWall" || other.transform.tag == "Obstacle")
        {
            Direction.x = -Direction.x;
            Direction.y = -Direction.y;
            //RB.velocity = -RB.velocity;

        }
        if(other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }
    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
        print("EnemySlime destroyed");
        Destroy(gameObject);
        Drop();
    }

    public void Move()
    {
        if (moveTimeCounter > MoveTime)
        {
            if (waitFlag)
            {
                StartCoroutine(Think());
            }
        }
        else
        {
            moveTimeCounter += Time.fixedDeltaTime;
            //RB.MovePosition(RB.position + Direction * Speed * Time.fixedDeltaTime);
            RB.AddForce(Direction * Stats.Speed);
        }
        
    }

    IEnumerator Think()
    {
        waitFlag = false;
        yield return new WaitForSeconds(Wait);
        waitFlag = true;
        Direction = IEnemy.Directions[rnd.Next(IEnemy.Directions.Length)];
        if (Direction == Vector2.up || Direction == Vector2.right)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        moveTimeCounter = 0;
    }

    public void Attack()
    {

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

    public void Drop()
    {
        if (rnd.Next(100) < Stats.DropChance)
        {
            Instantiate(Stats.DropObjects[rnd.Next(Stats.DropObjects.Count)], transform.position, Quaternion.identity);
        }
    }
}
