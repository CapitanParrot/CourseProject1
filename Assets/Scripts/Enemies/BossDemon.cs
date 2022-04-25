using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDemon : MonoBehaviour, IEnemy
{
    public EnemyStats Stats;
    public Rigidbody2D RB;
    public Vector2 Direction;
    public float MoveTime = 1;

    public float moveTimeCounter = 0;

    public int Wait = 3;

    //private bool waitFlag = true;

    public float EnemyKnockback = 10;

    private System.Random rnd;
    public int DropCount = 3;
    public string BossName = "Demon General";
    public int DashImpulse = 5000;

    // p
    public bool stunned = false;
    private bool secondPhase = false;
    public int AttackTime = 10;
    public int StunTime = 2;
    public float MicroStunTime = 0.5f;
    public int currentHealth;
    
    //p
    public float attackTimeCounter = 0;
    void Awake()
    {
        //rnd = GameManager.Instance.Rnd;
        rnd = new System.Random();
        currentHealth = Stats.Health;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Wall" || other.transform.tag == "FightWall" || other.transform.tag == "Obstacle")
        {
            Direction.x = -Direction.x;
            Direction.y = -Direction.y;
            //RB.velocity = -RB.velocity;

        }
        if (other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }
    public void Attack()
    {
        var dashDir = GameManager.Instance.PlayerInstance.transform.position - transform.position;
        
        RB.AddForce(dashDir.normalized * DashImpulse);
        attackTimeCounter = 0;
        if (dashDir.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator Stun(float time)
    {
        stunned = true;
        yield return new WaitForSeconds(time);
        stunned = false;
    }

    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
        print("BossDemon destroyed");
        Destroy(gameObject);
        Drop();
    }

    public void Drop()
    {
        for (int i = 0; i < DropCount; i++)
        {
            Instantiate(Stats.DropObjects[rnd.Next(Stats.DropObjects.Count)], transform.position, Quaternion.identity,GameManager.Instance.LevelCreatorInstance.transform);
        }
    }

    public void Move()
    {
        if (moveTimeCounter > MoveTime)
        {
            Direction = IEnemy.Directions[rnd.Next(IEnemy.Directions.Length)];
            if (Direction == Vector2.up || Direction == Vector2.right)
            {
                print("turn straight");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                print("turn back");
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            moveTimeCounter = 0;
        }
        else
        {
            moveTimeCounter += Time.fixedDeltaTime;
            //RB.MovePosition(RB.position + Direction * Speed * Time.fixedDeltaTime);
            RB.AddForce(Direction * Stats.Speed);
        }
    }

    public void TakeDamage(int damage, Transform attacker, float knockback)
    {
        currentHealth -= damage;
        UIManager.Instance.ChangeBossHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Death();
            UIManager.Instance.DeactivateBossHealth();
        } else if(currentHealth <= Stats.Health / 2)
        {
            secondPhase = true;
            AttackTime = 5;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Direction = IEnemy.Directions[rnd.Next(4)];
        EnemyManager.Instance.AddEnemy();
        UIManager.Instance.DrawBossHealth(BossName, Stats.Health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (!stunned)
        {
            if(attackTimeCounter > AttackTime)
            {
                Attack();
                StartCoroutine(Stun(StunTime));

            }
            else
            {
                attackTimeCounter += Time.fixedDeltaTime;
            }

            Move();
            
        }
    }
}
