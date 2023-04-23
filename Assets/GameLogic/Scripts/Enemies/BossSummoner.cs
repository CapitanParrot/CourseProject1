using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Второй босс Призыватель мертвых.
// Собственно призывает мертвых по таймеру.
// Призывает Скелетов и Зомби.
// Кажется, что босс довольно сложный, но мне нравится его динамика.
// Во второй фазе перезарядка призыва уменьшается.
public class BossSummoner : MonoBehaviour, IEnemy
{
    public EnemyStats Stats;
    public Rigidbody2D RB;
    public Vector2 Direction;
    public float MoveTime = 1;

    private float moveTimeCounter = 0;

    private System.Random rnd;
    public int DropCount = 3;
    public string BossName = "Призыватель мертвых";

    // Босс сам является спавнером врагов.
    public EnemySpawner Spawner;

    // Враги, которых будет призывать босс.
    public List<GameObject> SpawnEnemies;

    private bool isReady = true;
    
    // Минимальное число врагов в комнате для следующего призыва,
    // То есть если остался босс и один миньон, босс призывает еще раз.
    public int MinMinionsCount = 2;
    private bool secondPhase = false;
    public int ReloadTime = 5;
    public int currentHealth;

    public Animator Animator;
    public AudioSource AudioSource;
    public AudioClip TakeDamageSound;

    public void Attack()
    {
        // Босс не может заспавнить всю комнату миньонами. При MinMinionsCount = 2 максимум 4 миньона.
        if (isReady && EnemyManager.Instance.GetEnemyCount() <= MinMinionsCount)
        {
            Spawner.Spawn(SpawnEnemies);
            StartCoroutine(Reload(ReloadTime));
        }
    }

    // Перезарядка атаки.
    IEnumerator Reload(int time)
    {
        isReady = false;
        yield return new WaitForSeconds(time);
        isReady = true;
    }

    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
        UIManager.Instance.DeactivateBossHealth();
        print("BossSummoner destroyed");
        Destroy(gameObject);
        Drop();
    }

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
        // Двигается в случайных направлениях по таймеру.
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

    // Метод получения урона.
    public void TakeDamage(int damage, Transform attacker, float knockback)
    {
        currentHealth -= damage;
        UIManager.Instance.ChangeBossHealth(currentHealth);
        Animator.SetTrigger("Hit");
        AudioSource.PlayOneShot(TakeDamageSound);
        if (currentHealth <= 0)
        {
            Death();

        }
        else if (currentHealth <= Stats.Health / 2)
        {
            secondPhase = true;
            ReloadTime = 2;
        }
    }

    // Сталкиваемя со стенкой разворачиваемся.
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Wall" || other.transform.tag == "FightWall" || other.transform.tag == "Obstacle")
        {
            Direction.x = -Direction.x;
            Direction.y = -Direction.y;

        }
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

        // Сталкиваемся с игроком, бьем игрока.
        if (other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }

    void Awake()
    {
        rnd = new System.Random();
        currentHealth = Stats.Health;
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
        Move();
        Attack();
        Animator.SetFloat("Speed", RB.velocity.magnitude);
    }
}
