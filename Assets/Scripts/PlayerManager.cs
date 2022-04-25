using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Animator Animator;

    public static PlayerManager Instance;
    
    public IWeapon ActiveWeapon;
    
    public int CurrentHealth = 3;
    public int MaxHealth = 3;
    public Rigidbody2D RB;

    public GameObject WeaponRotation;
    // Start is called before the first frame update
    void Start()
    {
        
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    if (transform.GetChild(i).tag.Equals("Rotation"))
        //    {
        //        WeaponRotation = transform.GetChild(i).gameObject;
        //    }
        //}
        UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
    }
    void Awake()
    {
        Instance = this;
        ActiveWeapon = GetComponentInChildren<IWeapon>();
    }
    public void TakeDamage(int dmg)
    {
        Animator.SetTrigger("Hit");
        CurrentHealth -= dmg;
        UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void AddMaxHealth(int amount)
    {
        MaxHealth += amount;
        UIManager.Instance.DrawHealth(CurrentHealth, MaxHealth);
    }

    public void Heal(int amount)
    {
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
        print("Player dead");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Exit")
        {
            GameManager.Instance.NextLevel();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
