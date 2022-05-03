using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// Слизень - самый крутой враг первого этажа
// Непредсказуемый, толстый, но ценный.
// Нет атаки
// Двигается в случайных направлениях.
public class EnemySlime : MonoBehaviour, IEnemy
{
    public EnemyStats Stats;
    private int currentHealth;

    public Rigidbody2D RB;

    public Vector2 Direction;

    private System.Random rnd;

    public float MoveTime = 1;

    public float moveTimeCounter = 0;

    public int Wait = 3;

    private bool waitFlag = true;

    public Animator Animator;

    public AudioSource AudioSource;
    public AudioClip TakeDamageSound;

    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
    }

    void Start()
    {
        Direction = IEnemy.Directions[rnd.Next(4)];
        EnemyManager.Instance.AddEnemy();
        currentHealth = Stats.Health;
        AudioManager.Instance.AddSource(AudioSource);
    }

    public void TakeDamage(int dmg, Transform attacker, float knockback)
    {
        Push(transform.position - attacker.position, knockback);
        currentHealth -= dmg;
        Animator.SetTrigger("Hit");
        AudioSource.PlayOneShot(TakeDamageSound);
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
        }

        // Склизкая гадость наносит урон.
        if(other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }
    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
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
            RB.AddForce(Direction * Stats.Speed);
        }
        Animator.SetFloat("Speed",RB.velocity.magnitude);
    }

    // Думает также как и летающее нечто,
    // Но его судьба не предопределена паттерном.
    // Слизень свободен в своих движениях.
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

    public void Attack() {}

    void FixedUpdate()
    {
        Move();
    }

    public void Drop()
    {
        if (rnd.Next(100) < Stats.DropChance)
        {
            Instantiate(Stats.DropObjects[rnd.Next(Stats.DropObjects.Count)], transform.position, Quaternion.identity,
                GameManager.Instance.LevelCreatorInstance.transform);
        }
    }
}
