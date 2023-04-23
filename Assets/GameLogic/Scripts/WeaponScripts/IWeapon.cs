using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Интерфейс оружия.
public interface IWeapon
{
    string Name { get; }
    string Description { get; }
    public void Attack();
    public float GetAttackSpeed();
}
