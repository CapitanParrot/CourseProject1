using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Управляет состоянием персонажа игрока.
public class PlayerManager : MonoBehaviour
{
    public Animator Animator;

    public static PlayerManager Instance;
    
    public IWeapon ActiveWeapon;
    
    public int CurrentHealth = 3;
    public int MaxHealth = 3;
    public int BonusDamage = 0;
    public float MaxSpeed = 7f;
    public float BonusAttackSpeed = 0f;
    public float MaxAttackSpeed = 2f;
    public int InvincibilityTime = 1;
    private bool isPlayerInvincible = false;

    public Rigidbody2D RB;

    public GameObject WeaponRotation;

    public AudioClip TakeDamageSound;
    public AudioClip HealSound;
    public AudioSource AudioSource;

    void Start()
    {
        UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
        AudioManager.Instance.AddSource(AudioSource);
    }

    void Awake()
    {
        Instance = this;
        ActiveWeapon = GetComponentInChildren<IWeapon>();
    }

    // Здесь игрок получает урон.
    public void TakeDamage(int dmg)
    {
        if (!isPlayerInvincible)
        {
            Animator.SetTrigger("Hit");
            CurrentHealth -= dmg;
            UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
            AudioSource.PlayOneShot(TakeDamageSound);
            if (CurrentHealth <= 0)
            {
                Death();
            }

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
        UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
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
        if (CurrentHealth + amount >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth += amount;
        }
        UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
    }

    public void Death()
    {
        GameManager.Instance.DeathCounter++;
        UIManager.Instance.ShowDeathScreen();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Exit")
        {
            GameManager.Instance.NextLevel();
        }
    }
}
