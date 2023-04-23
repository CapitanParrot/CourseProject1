using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Шаман a.k.a бойся если их 4
// Очень умный враг, стоит на месте и стреляет в игрока огненными шарами, если его видит
// Если не видит, ходит в случайных направлениях.
// Монстр 3 этажа.
public class EnemyShaman : MonoBehaviour, IEnemy
{
    private int currentHealth;
    public EnemyStats Stats;
    
    private System.Random rnd;

    public Rigidbody2D RB;

    public Vector2 Direction;

    public float MoveTime = 1;

    private float moveTimeCounter = 0;

    // Эту штуку будет стрелять в игрока.
    public GameObject Projectile;
    public int ProjectileSpeed = 10;

    public int ReloadTime = 2;

    public bool isReady = true;

    public bool canMove = true;

    private int hitMask;
    private float hitDistance = 15f;

    public Animator Animator;
    public AudioSource AudioSource;
    public AudioClip TakeDamageSound;


    public void Attack()
    {
        // Сначала пускает луч в игрока.
        RaycastHit2D hit =
            Physics2D.Raycast(transform.position,
                (GameManager.Instance.PlayerInstance.transform.position - transform.position).normalized, hitDistance,
                hitMask);

        // Если попал в игрока останавливается и стреляет снарядом, если нет двигается.
        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                canMove = false;
                if (isReady)
                {
                    Vector2 shotDir = (hit.transform.position - transform.position).normalized;
                    Fireball fireball = Instantiate(Projectile, transform.position, Quaternion.identity)
                        .GetComponent<Fireball>();
                    fireball.Direction = shotDir;
                    fireball.Damage = Stats.Damage;
                    fireball.Speed = ProjectileSpeed;
                    fireball.Fire();
                    StartCoroutine(Reload(ReloadTime));
                }
            }
            else
            {
                canMove = true;
            }
        }
    }

    // Перезарядка стрельбы.
    IEnumerator Reload(int time)
    {
        isReady = false;
        yield return new WaitForSeconds(time);
        isReady = true;
    }

    public void Death()
    {
        EnemyManager.Instance.SubtractEnemy();
        print("Shaman destroyed");
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

    public void Move()
    {
        // Двигается в случайных направлениях.
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

    void Start()
    {
        Direction = IEnemy.Directions[rnd.Next(4)];
        EnemyManager.Instance.AddEnemy();
        currentHealth = Stats.Health;
        AudioManager.Instance.AddSource(AudioSource);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag.Equals("Wall") || other.transform.tag == "FightWall" || other.transform.tag == "Obstacle")
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

        // Мало стрельбы, он и вблизи навалять может.
        if (other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }

    void Awake()
    {
        rnd = new System.Random();
        currentHealth = Stats.Health;
        int obstacleMask = 1 << LayerMask.NameToLayer("Obstacle");
        int playerMask = 1 << LayerMask.NameToLayer("Player");
        hitMask = obstacleMask | playerMask;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
        Animator.SetFloat("Speed", RB.velocity.magnitude);
        Attack();
        
    }
}
