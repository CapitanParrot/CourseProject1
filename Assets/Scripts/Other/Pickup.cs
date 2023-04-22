using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ����� ������ ����������� ���������.
public class Pickup : MonoBehaviour
{
    public string PickupName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Invoke(PickupName, 0);
        }
    }

    // ��������� �������.
    private void SmallFlask()
    {
        if (PlayerManager.Instance.currentHealth < PlayerManager.Instance.maxHealth)
        {
            PlayerManager.Instance.Heal(1);
            Destroy(gameObject);
        }
    }

    // ������� �������.
    private void BigFlask()
    {
        if (PlayerManager.Instance.currentHealth < PlayerManager.Instance.maxHealth)
        {
            PlayerManager.Instance.Heal(2);
            Destroy(gameObject);
        }
    }
}
