using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ”правл€ет состо€нием персонажа игрока.
public class PlayerManager : MonoBehaviour
{
    public Animator Animator;

    

    public static PlayerManager Instance;
    
    public IWeapon ActiveWeapon;
    
    public int currentHealth = 3;

    // NEW ƒобавил свойства дл€ полей здоровь€ дл€ удобного отслеживани€
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            if(value < currentHealth)
            {
                currentHealth = value;
                if (currentHealth <= 0)
                {
                    Death();
                }
            }
            else
            {
                currentHealth = value;
                if (currentHealth >= maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }
        }
    }

    public int maxHealth = 4;

    // NEW ƒобавил свойства дл€ полей здоровь€ дл€ удобного отслеживани€
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            if(value > 20) 
            {
                maxHealth = 20;
            }
            else
            {
                maxHealth = value;
            }

            if(value < 0)
            {
                if (CurrentHealth > value )
                {
                    CurrentHealth = value;
                }
            }
        }
    }

    public int BonusDamage = 0;

    public float MaxSpeed = 7f;
    public float MinSpeed = 3f;
    
    public float BonusAttackSpeed = 0f;
    public float MaxAttackSpeed = 2f;
    
    public int InvincibilityTime = 1;
    
    private bool isPlayerInvincible = false;

    public Rigidbody2D RB;

    public GameObject WeaponRotation;

    public AudioClip TakeDamageSound;
    public AudioClip HealSound;
    public AudioSource AudioSource;

    // NEW —обытие дл€ инициализации игрока, чтобы все кому надо подписались на другие событи€
    public event EventHandler<InitPlayerArgs> InitPlayer;

    // NEW —обытие изменени€ здоровь€
    public event EventHandler<PlayerHealthArgs> ChangeHealth;

    // NEW —обытие смерти игрока
    public event EventHandler<PlayerDeathArgs> PlayerDeathEvent;
    void Start()
    {
        InitPlayer?.Invoke(this,new InitPlayerArgs(MaxHealth, CurrentHealth, AudioSource));
        //UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
        //AudioManager.Instance.AddSource(AudioSource);
    }

    void Awake()
    {
        Instance = this;
        ActiveWeapon = GetComponentInChildren<IWeapon>();
    }

    // «десь игрок получает урон.
    public void TakeDamage(int dmg)
    {
        if (!isPlayerInvincible)
        {
            Animator.SetTrigger("Hit");
            CurrentHealth -= dmg;
            print(CurrentHealth);
            print(MaxHealth);
            ChangeHealth?.Invoke(this, new PlayerHealthArgs(CurrentHealth, MaxHealth));
            //UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
            AudioSource.PlayOneShot(TakeDamageSound);
            /*if (currentHealth <= 0)
            {
                Death();
            }*/

            StartCoroutine(Invincible(InvincibilityTime));
        }
    }

    IEnumerator Invincible(int time)
    {
        isPlayerInvincible = true;
        yield return new WaitForSeconds(time);
        isPlayerInvincible = false;
    }

    public void AddMaxHealth(int amount)
    {
        MaxHealth += amount;
        ChangeHealth?.Invoke(this, new PlayerHealthArgs(CurrentHealth, MaxHealth));
        //UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
    }

    public void RemoveMaxHealth(int amount)
    {  
        MaxHealth -= amount;
        ChangeHealth?.Invoke(this, new PlayerHealthArgs(CurrentHealth, MaxHealth));
    }

    public void AddBonusDamage(int amount)
    {
        BonusDamage += amount;
    }

    public void AddSpeed(float amount)
    {
        var pm = gameObject.GetComponent<PlayerMovement>();
        if (pm.Speed + amount > MaxSpeed)
        {
            pm.Speed = MaxSpeed;
        }
        else
        {
            pm.Speed += amount;
        }
    }

    public void SubSpeed(float amount)
    {
        var pm = gameObject.GetComponent<PlayerMovement>();
        if(pm.Speed - amount < MinSpeed)
        {
            pm.Speed = MinSpeed;
        }
        else
        {
            pm.Speed -= amount;
        }
    }

    public void AddAttackSpeed(float amount)
    {
        if (BonusAttackSpeed + amount > MaxAttackSpeed)
        {
            BonusAttackSpeed = MaxAttackSpeed;
        }
        else
        {
            BonusAttackSpeed += amount;
        }
    }

    public void Heal(int amount)
    {
        AudioSource.PlayOneShot(HealSound);
        CurrentHealth += amount;
        ChangeHealth?.Invoke(this, new PlayerHealthArgs(CurrentHealth, MaxHealth));
        //UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
    }

    public void Death()
    {
        PlayerDeathEvent?.Invoke(this, new PlayerDeathArgs());
        //GameManager.Instance.DeathCounter++;
        //UIManager.Instance.ShowDeathScreen();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Exit")
        {
            GameManager.Instance.NextLevel();
        }
    }
}
