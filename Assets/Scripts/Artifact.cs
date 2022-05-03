using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Artifact : MonoBehaviour
{
    // �������� ������, ������� ������ �������� ��������.
    public string MethodName;

    private bool isPlayerClosely = false;

    void Update()
    {
        if (isPlayerClosely)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ArtifactManager.Instance.PlaySound();
                Invoke(MethodName, 0);
                Destroy(gameObject);
            }
        }
    }

    // ����� ����� �������� � ���� ���������, ��� ��������� ���������� ������.
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

    // � ����� ������, ������ ���������.
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

    void Baton()
    {
        PlayerManager.Instance.AddMaxHealth(2);
        PlayerManager.Instance.Heal(2);
        UIManager.Instance.SetArtifactDescription("����� �����", "+ �������� ��������");
    }

    void Whetstone()
    {
        PlayerManager.Instance.AddBonusDamage(1);
        UIManager.Instance.SetArtifactDescription("��������� ������", "+ ����");
    }

    void GoldenSword()
    {
        GameManager.Instance.FinishGame();
    }
    void Meetballs()
    {
        PlayerManager.Instance.AddMaxHealth(4);
        PlayerManager.Instance.Heal(4);
        UIManager.Instance.SetArtifactDescription("�������", "++ �������� ��������");
    }
    void Boots()
    {
        PlayerManager.Instance.AddSpeed(0.1f);
        UIManager.Instance.SetArtifactDescription("������� �������", "+ ��������");
    }

    void Glove()
    {
        PlayerManager.Instance.AddAttackSpeed(0.2f);
        UIManager.Instance.SetArtifactDescription("������� ��������", "+ �������� �����");
    }
}
