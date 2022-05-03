using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Artifact : MonoBehaviour
{
    // Название метода, который должен вызывать артефакт.
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

    // Когда Игрок подходит в зону колайдера, над предметом появляется иконка.
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

    // А когда уходит, иконка пропадает.
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
        UIManager.Instance.SetArtifactDescription("Булка хлеба", "+ максимум здоровья");
    }

    void Whetstone()
    {
        PlayerManager.Instance.AddBonusDamage(1);
        UIManager.Instance.SetArtifactDescription("Точильный камень", "+ урон");
    }

    void GoldenSword()
    {
        GameManager.Instance.FinishGame();
    }
    void Meetballs()
    {
        PlayerManager.Instance.AddMaxHealth(4);
        PlayerManager.Instance.Heal(4);
        UIManager.Instance.SetArtifactDescription("Тефтели", "++ максимум здоровья");
    }
    void Boots()
    {
        PlayerManager.Instance.AddSpeed(0.1f);
        UIManager.Instance.SetArtifactDescription("Удобные ботинки", "+ скорость");
    }

    void Glove()
    {
        PlayerManager.Instance.AddAttackSpeed(0.2f);
        UIManager.Instance.SetArtifactDescription("Удобная перчатка", "+ скорость атаки");
    }
}
