using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��������� ������� ��������� �������� ������
public class PlayerHealthArgs : EventArgs
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public PlayerHealthArgs(int currentHealth, int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
    }
}
