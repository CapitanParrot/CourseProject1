using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


// —келет, самый веселый враг, как по мне
// Ќет атаки.
// ѕри спавне выбирает случайное направление.
// ≈сли сталкиваетс€ с чем нибудь, разворачиваетс€ и топает в противоположную сторону.
public class EnemySkeleton : MonoBehaviour, IEnemy
{
    public EnemyStats Stats;
    private int currentHealth;

    public Rigidbody2D RB;

    public Vector2 Direction;

    private Random rnd;

    public Animator Animator;

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
        Animator.SetTrigger("Hit");
        AudioSource.PlayOneShot(TakeDamageSound);
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
        Direction = -Direction;
        ChangeSpriteSide();
    }

    private void ChangeSpriteSide()
    {
        if (Direction == Vector2.up || Direction == Vector2.right)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
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
