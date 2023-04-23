using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;


// Зомби a.k.a бойся если тебя зажали.
// Нет атаки
// Сразу бежит к игроку и пытается врезаться.
// Использует библиотеку A*.
public class EnemyZombie : MonoBehaviour,IEnemy
{
    public AIDestinationSetter DestinationSetter;
    public AIPath AiPath;
    private int currentHealth;
    public EnemyStats Stats;
    public float SlowSpeed = 1f;
    public float NormalSpeed = 3f;
    public int SlowTime = 1;
    private System.Random rnd;
    public Animator Animator;

    public Rigidbody2D RB;

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
        print("Zombie destroyed");
        Destroy(gameObject);
        Drop();
    }

    public void Drop()
    {
        if (rnd.Next(100) < Stats.DropChance)
        {
            Instantiate(Stats.DropObjects[rnd.Next(Stats.DropObjects.Count)], transform.position, Quaternion.identity,
                GameManager.Instance.LevelCreatorInstance.transform);
        }
    }

    // За движение отвечают компоненты из библиотеки A*.
    public void Move() {}

    public void TakeDamage(int damage, Transform attacker, float knockback)
    {
        currentHealth -= damage;
        Animator.SetTrigger("Hit");
        AudioSource.PlayOneShot(TakeDamageSound);
        if (currentHealth <= 0)
        {
            Death();
        }

        StartCoroutine(SlowDown(SlowTime));
    }

    // Если получил по лицу, замедляется на время, без этого получалось слишком сложно.
    IEnumerator SlowDown(int time)
    {
        AiPath.maxSpeed = SlowSpeed;
        yield return new WaitForSeconds(time);
        AiPath.maxSpeed = NormalSpeed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }

    void Start()
    {
        EnemyManager.Instance.AddEnemy();
        currentHealth = Stats.Health;
        DestinationSetter.target = GameManager.Instance.PlayerInstance.transform;
        AudioManager.Instance.AddSource(AudioSource);
    }

    void Update()
    {
        // Разворачиваем справйт в сторону движения.
        if(AiPath.desiredVelocity.x >= 0.01f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else if(AiPath.desiredVelocity.x <= -0.01f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
