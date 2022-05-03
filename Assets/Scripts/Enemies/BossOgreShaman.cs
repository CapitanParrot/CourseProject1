using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Третий босс.
// Ходит в случайных направлениях.
// Стрелят в игрока тремя снарядами через равные промежутки времени.
// Во второй фазе дополнительно раз в две атаки стреляет 12 снарядов вокруг себя.
// После круговой атаки перезарядка больше чем у обычной.
public class BossOgreShaman : MonoBehaviour,IEnemy
{
    public EnemyStats Stats;
    public Rigidbody2D RB;
    public Vector2 Direction;
    public float MoveTime = 1;
    private float moveTimeCounter = 0;
    private System.Random rnd;

    public int DropCount = 3;
    public string BossName = "Огр Шаман";

    public GameObject Projectile;
    public int ProjectileSpeed;
    public float FirstAttackAngle = 30f;

    // Влияет на количество снарядов во второй атаке.
    public float SecondAttackAngle = 30f;

    private bool isReady = true;
    private bool secondPhase = false;
    public int ReloadTimeFirst = 2;
    public int ReloadTimeSecond = 8;
    
    public int currentHealth;
    private int AttackCounter = 0;

    public AudioSource AudioSource;
    public AudioClip TakeDamageSound;

    public Animator Animator;
    
    public void Attack()
    {
        // Атака происходит после перезарядки.
        if (isReady)
        {
            if (!secondPhase)
            {
                FirstAttack();
                StartCoroutine(Reload(ReloadTimeFirst));
            }
            else
            {
                if (AttackCounter < 2)
                {
                    FirstAttack();
                    StartCoroutine(Reload(ReloadTimeFirst));
                    AttackCounter++;
                }
                else
                {
                    SecondAttack();
                    StartCoroutine(Reload(ReloadTimeSecond));
                    AttackCounter = 0;
                }
            }
        }
    }

    private void SecondAttack()
    {
        // Немного тригонометрии с вычислением углов.
        float cs = Mathf.Cos(Mathf.Deg2Rad * SecondAttackAngle);
        float sn = Mathf.Sin(Mathf.Deg2Rad * SecondAttackAngle);

        Vector2 shotDir = Vector2.down;
        for (int i = 0; i < 360 / SecondAttackAngle; i++)
        {
            ShotFireball(shotDir);
            shotDir = new Vector2(shotDir.x * cs - shotDir.y * sn, shotDir.x * sn + shotDir.y * cs);
        }
    }

    private void FirstAttack()
    {
        // Выпускаем три снаряда, один прямо в игрока и два побокам.
        float cs1 = Mathf.Cos(Mathf.Deg2Rad * FirstAttackAngle);
        float sn1 = Mathf.Sin(Mathf.Deg2Rad * FirstAttackAngle);

        float cs2 = Mathf.Cos(Mathf.Deg2Rad * -FirstAttackAngle);
        float sn2 = Mathf.Sin(Mathf.Deg2Rad * -FirstAttackAngle);

        // Вектор направление от босса к игроку.
        Vector2 shotDir1 = (GameManager.Instance.PlayerInstance.transform.position - transform.position).normalized;

        Vector2 shotDir2 = new Vector2(shotDir1.x * cs1 - shotDir1.y * sn1, shotDir1.x * sn1 + shotDir1.y * cs1);
        Vector2 shotDir3 = new Vector2(shotDir1.x * cs2 - shotDir1.y * sn2, shotDir1.x * sn2 + shotDir1.y * cs2);
        ShotFireball(shotDir1);
        ShotFireball(shotDir2);
        ShotFireball(shotDir3);
    }

    // устанавливает все данные для снаряда.
    private void ShotFireball(Vector2 dir)
    {
        Fireball fireball1 = Instantiate(Projectile, transform.position, Quaternion.identity)
            .GetComponent<Fireball>();
        fireball1.Direction = dir;
        fireball1.Damage = Stats.Damage;
        fireball1.Speed = ProjectileSpeed;
        fireball1.Fire();
    }

    // Таймер перезарядки.
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
        print("BossOgreShaman destroyed");
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
        // Двигается всегда, даже в перезарядке.
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
            RB.AddForce(Direction * Stats.Speed);
        }
    }

    // Босс получает по своей морде.
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
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Wall" || other.transform.tag == "FightWall" || other.transform.tag == "Obstacle")
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
        if (other.transform.tag == "Player")
        {
            PlayerManager.Instance.TakeDamage(Stats.Damage);
        }
    }

    void Start()
    {
        Direction = IEnemy.Directions[rnd.Next(4)];
        EnemyManager.Instance.AddEnemy();
        UIManager.Instance.DrawBossHealth(BossName, Stats.Health);
        AudioManager.Instance.AddSource(AudioSource);
    }

    void Awake()
    {
        rnd = new System.Random();
        currentHealth = Stats.Health;
    }

    void FixedUpdate()
    {
        Move();
        Attack();
        Animator.SetFloat("Speed", RB.velocity.magnitude);
    }
}
