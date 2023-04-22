using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;

// ���������� ���������� ���������.
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D RB2D;

    public float Speed = 6f;

    public Camera cam;
    private Vector2 movement;

    private Vector2 mousePos;

    // ����� �������� ������.
    private Transform WeaponRotation;

    public float Radius = 1;

    public float angle;

    private float nextAttackTime = 0f;

    private PlayerManager playerManager;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        WeaponRotation = playerManager.WeaponRotation.transform;
        RB2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        playerManager.Animator.SetFloat("Speed",Mathf.Abs(movement.x) + Mathf.Abs(movement.y));

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // �������� ����� �������.
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerManager.ActiveWeapon.Attack();
                nextAttackTime = Time.time + 1f / (playerManager.ActiveWeapon.GetAttackSpeed() + playerManager.BonusAttackSpeed);
            }
        }
    }

    void FixedUpdate()
    {
        // �������� ������ ����� �� ������.
        RB2D.MovePosition(RB2D.position + movement * Speed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - RB2D.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        WeaponRotation.eulerAngles = new Vector3(0,0,angle);

        // �������� ������� ��������� � ������� �������.
        if (angle <= 0 && angle >= -180) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
