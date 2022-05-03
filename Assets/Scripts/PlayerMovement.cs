using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;

// Управление движениями персонажа.
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D RB2D;

    public float Speed = 6f;

    public Camera cam;
    private Vector2 movement;

    private Vector2 mousePos;

    // Точка кручения оружия.
    private Transform WeaponRotation;

    public float Radius = 1;

    public float angle;

    private float nextAttackTime = 0f;


    void Start()
    {
        WeaponRotation = PlayerManager.Instance.WeaponRotation.transform;
        RB2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        PlayerManager.Instance.Animator.SetFloat("Speed",Mathf.Abs(movement.x) + Mathf.Abs(movement.y));

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Задрежка атаки оружием.
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerManager.Instance.ActiveWeapon.Attack();
                nextAttackTime = Time.time + 1f / (PlayerManager.Instance.ActiveWeapon.GetAttackSpeed() + PlayerManager.Instance.BonusAttackSpeed);
            }
        }
    }

    void FixedUpdate()
    {
        // Вращение оружия вслед за мышкой.
        RB2D.MovePosition(RB2D.position + movement * Speed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - RB2D.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        WeaponRotation.eulerAngles = new Vector3(0,0,angle);

        // Разворот спрайта персонажа в сторону курсора.
        if (angle <= 0 && angle >= -180) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
