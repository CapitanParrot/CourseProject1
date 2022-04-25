using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword : MonoBehaviour, IWeapon
{
    public Weapon Parameters;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask EnemyLayers;
    
    public Animator Animator;

    private bool isPlayerClosely = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClosely)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pressed E");
                GameObject oldWeapon = null;
                for (int i = 0; i < PlayerManager.Instance.WeaponRotation.transform.childCount; i++)
                {
                    if (PlayerManager.Instance.WeaponRotation.transform.GetChild(i).tag.Equals("Weapon"))
                    {
                        oldWeapon = PlayerManager.Instance.WeaponRotation.transform.GetChild(i).gameObject;
                    }
                }

                if (oldWeapon != null)
                {
                    oldWeapon.transform.parent = null;
                    oldWeapon.GetComponent<BoxCollider2D>().enabled = true;
                    oldWeapon.transform.position = transform.position;
                    oldWeapon.transform.localScale = new Vector3(1, 1, 1);
                }

                gameObject.transform.parent = PlayerManager.Instance.WeaponRotation.transform;
                gameObject.SetActive(false);
                gameObject.transform.localPosition = new Vector3(0, 0.3f,0);
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                PlayerManager.Instance.ActiveWeapon = this;
                gameObject.SetActive(true);
                isPlayerClosely = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            isPlayerClosely = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag.Equals("PressE"))
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            isPlayerClosely = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag.Equals("PressE"))
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public void Attack()
    {
        Animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
        int hitCounter = 0;
        foreach (var enemy in hitEnemies)
        {
            if (hitCounter >= Parameters.EnemiesPerHit) break;
            enemy.GetComponent<IEnemy>().TakeDamage(Parameters.Damage, transform, Parameters.Knockback);
        }
    }
    public float GetAttackSpeed()
    {
        return Parameters.AttackSpeed;
    }

    void OnDrawGizmosSelected()
    {
        if(AttackPoint == null) return;

        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
