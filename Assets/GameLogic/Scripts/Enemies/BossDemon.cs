using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
// ����� � ��������� ������� � �� ������ ������ ����� � ������� ������ � ��������.
// ������ ���� ��������� � �������� ��������, � ��� ������� ����� ����.
public class BossDemon : MonoBehaviour, IEnemy
{
    public EnemyStats Stats;
    public Rigidbody2D RB;
    public Vector2 Direction;
    public float MoveTime = 1;

    private float moveTimeCounter = 0;

    private System.Random rnd;
    public int DropCount = 3;
    public string BossName = "����� �������";
    public int DashImpulse = 5000;

    private bool stunned = false; 
    public int AttackTime = 10;
    public int StunTime = 2;
    private int currentHealth;
    
    private float attackTimeCounter = 0;

    public Animator Animator;

    public AudioSource AudioSource;
    public AudioClip TakeDamageSound;
    
    void Awake()
    {
        rnd = new System.Random();
        currentHealth = Stats.Health;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // ���� ������������ �� ������ ��������� � ��������������� �������.
        if (other.transform.tag.Equals("Wall") || other.transform.tag.Equals("FightWall") || other.transform.tag.Equals("Obstacle"))
        {
            Direction.x = -Direction.x;
            Direction.y = -Direction.y;
        }
        if (Direction == Vector2.up || Direction == Vector2.right)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (other.transform.tag.Equals("Player"))
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }
    public void Attack()
    {
        // ������� ������ �� ����� �� ������.
        var dashDir = GameManager.Instance.PlayerInstance.transform.position - transform.position;
        
        RB.AddForce(dashDir.normalized * DashImpulse);
        attackTimeCounter = 0;

        // ������������� ������ � ������� �����.
        if (dashDir.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    // ������ �����.
    IEnumerator Stun(float time)
    {
        stunned = true;
        yield return new WaitForSeconds(time);
        stunned = false;
    }

    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
        UIManager.Instance.DeactivateBossHealth();
        print("BossDemon destroyed");
        Destroy(gameObject);
        Drop();
    }

    // ������� ��������� ��������.
    public void Drop()
    {
        for (int i = 0; i < DropCount; i++)
        {
            Instantiate(Stats.DropObjects[rnd.Next(Stats.DropObjects.Count)], transform.position, Quaternion.identity,
                GameManager.Instance.LevelCreatorInstance.transform);
        }
    }

    public void Move()
    {
        // ��������� � ����� ����������� ��������� �����, ����� �������� ���������� �����.
        if (moveTimeCounter > MoveTime)
        {
            Direction = IEnemy.Directions[rnd.Next(IEnemy.Directions.Length)];
            if (Direction == Vector2.up || Direction == Vector2.right)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            moveTimeCounter = 0;
        }
        else
        {
            moveTimeCounter += Time.fixedDeltaTime;
            RB.AddForce(Direction * Stats.Speed);
        }
    }

    // ��������� ����� ������.
    public void TakeDamage(int damage, Transform attacker, float knockback)
    {
        currentHealth -= damage;
        UIManager.Instance.ChangeBossHealth(currentHealth);
        Animator.SetTrigger("Hit");
        AudioSource.PlayOneShot(TakeDamageSound);
        
        if (currentHealth <= 0)
        {
            Death();
        } else if(currentHealth <= Stats.Health / 2)
        {
            AttackTime = 5;
        }
    }

    void Start()
    {
        Direction = IEnemy.Directions[rnd.Next(4)];
        EnemyManager.Instance.AddEnemy();
        UIManager.Instance.DrawBossHealth(BossName, Stats.Health);
        AudioManager.Instance.AddSource(AudioSource);
    }

    void FixedUpdate()
    {
        // ���� ���� � �����, �� �� ����� ���������.
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

        Animator.SetFloat("Speed", RB.velocity.magnitude);
    }
}
