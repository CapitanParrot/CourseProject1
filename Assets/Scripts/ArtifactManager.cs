using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

// Генерирует артефакты.
public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance;
    
    public List<GameObject> Weapons;
    public List<GameObject> Artifacts;

    // Шанс, что выпадет оружие, а не предмет.
    public int WeaponChance;
    private System.Random rnd;

    public AudioSource AudioSource;
    public AudioClip PickupSound;

    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
        Instance = this;
    }

    void Start()
    {
        AudioManager.Instance.AddSource(AudioSource);
    }

    // Проигрывает звук при подбирании предмета.
    public void PlaySound()
    {
        AudioSource.PlayOneShot(PickupSound);
    }
    

    // Генерирует артефакт, любо оружие, лиюо предмет.
    public GameObject GetArtifact()
    {
        if (rnd.Next(101) <= WeaponChance)
        {
            int nextWeapon = rnd.Next(Weapons.Count);
            return Weapons[nextWeapon];
        }
        int nextArtifact = rnd.Next(Artifacts.Count);
        return Artifacts[nextArtifact];
    }
}
