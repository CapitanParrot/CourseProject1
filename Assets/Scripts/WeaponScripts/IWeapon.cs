using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Интерфейс оружия.
public interface IWeapon
{
    public void Attack();
    public float GetAttackSpeed();
}
