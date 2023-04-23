using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


// Мой самый любимый враг, летающее нечто с моделькой глаза с крыльями.
// Нет атаки, просто двигается согласно паттерну.
public class EnemyFlyingThing : MonoBehaviour, IEnemy
{
    private int currentHealth;
    public EnemyStats Stats;

    public Animator Animator;
    private System.Random rnd;
    public Rigidbody2D RB;

    public Vector2 Direction;
    private Vector2[] movePattern =
        {Vector2.up, Vector2.down, Vector2.down, Vector2.up, Vector2.right, Vector2.left, Vector2.left, Vector2.right};

    private int moveCounter = 0;
    public float MoveTime = 1;
    public float moveTimeCounter = 0;
    public int Wait = 3;
    private bool waitFlag = true;

    public AudioSource AudioSource;
    public AudioClip TakeDamageSound;

    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
    }
    public void Attack() {}

    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
        Destroy(gameObject);
        Drop();
    }

    // Двигается, немного думает, снова двигается.
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
    }

    // Думает какое направление ему выбрать.
    IEnumerator Think()
    {
        waitFlag = false;
        yield return new WaitForSeconds(Wait);
        waitFlag = true;

        // Выбор направления из паттерна.
        if (moveCounter >= movePattern.Length)
        {
            moveCounter = 0;
        }
        Direction = movePattern[moveCounter++];

        // Поворот спрайта в сторону движения.
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
    public void TakeDamage(int damage, Transform attacker, float knockback)
    {
        Push(transform.position - attacker.position, knockback);
        currentHealth -= damage;
        Animator.SetTrigger("Hit");
        AudioSource.PlayOneShot(TakeDamageSound);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    // Метод отталкивания.
    private void Push(Vector2 direction, float strength)
    {
        RB.AddForce(direction.normalized * strength, ForceMode2D.Impulse);
    }
    

    void Start()
    {
        EnemyManager.Instance.AddEnemy();
        Direction = movePattern[moveCounter++];
        currentHealth = Stats.Health;
        AudioManager.Instance.AddSource(AudioSource);
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }

    public void Drop()
    {
        if(rnd.Next(100) < Stats.DropChance)
        {
            Instantiate(Stats.DropObjects[rnd.Next(Stats.DropObjects.Count)], transform.position, Quaternion.identity,
                GameManager.Instance.LevelCreatorInstance.transform);
        }
    }
}
