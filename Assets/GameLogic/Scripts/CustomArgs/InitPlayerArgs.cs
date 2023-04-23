using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Аргументы события инициализации 
public class InitPlayerArgs : EventArgs
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public AudioSource AudioSource { get; set; }
    public InitPlayerArgs(int maxHealth, int currentHealth, AudioSource audioSource)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        AudioSource = audioSource;
    }
}
