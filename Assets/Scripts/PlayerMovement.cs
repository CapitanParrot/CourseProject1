using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D RB2D;

    public float Speed = 5f;

    public Camera cam;
    private Vector2 movement;

    private Vector2 mousePos;


    private Transform WeaponRotation;

    public Transform WeaponPoint;

    //public Transform Sword;

    public float Radius = 1;

    public float angle;

    private float nextAttackTime = 0f;

    // Update is called once per frame
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

        if(Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerManager.Instance.ActiveWeapon.Attack();
                nextAttackTime = Time.time + 1f / PlayerManager.Instance.ActiveWeapon.GetAttackSpeed();
            }
        }
        
    }

    void FixedUpdate()
    {
        // ¬ращение оружи€ вслед за мышкой.
        RB2D.MovePosition(RB2D.position + movement * Speed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - RB2D.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        WeaponRotation.eulerAngles = new Vector3(0,0,angle);
        WeaponPoint.RotateAround(WeaponRotation.localPosition,Vector3.zero, angle);

        // –азворот спрайта персонажа в сторону курсора.
        if (angle <= 0 && angle >= -180) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
