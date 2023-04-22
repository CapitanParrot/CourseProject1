using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
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

    public List<GameObject> Inventory = new List<GameObject>();

    // NEW Событие подбора артефакта
    public event EventHandler<ArtifactArgs> PickArtifact;

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
    

    // Генерирует артефакт, любо оружие, либо предмет.
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

    /// <summary>
    /// Чтобы артфакты не вызывали напрямую методы из кучи менеджеров, сделан этот метод
    /// </summary>
    /// <param name="artifactObject"></param>
    public void PlayArtifact(GameObject artifactObject)
    {
        PlaySound();
        IArtifact artifact = artifactObject.GetComponent<IArtifact>();
        artifact.Action(new Managers(PlayerManager.Instance, GameManager.Instance));
        PickArtifact?.Invoke(this, new ArtifactArgs(artifact.Name, artifact.Description));
        Inventory.Add(artifactObject);
    }
    /// <summary>
    /// Чтобы оружия не вызывали напрямую методы из кучи менеджеров, сделан этот метод
    /// </summary>
    /// <param name="weaponObject"></param>
    public void PlayWeapon(GameObject weaponObject)
    {
        PlaySound();
        IWeapon weapon = weaponObject.GetComponent<IWeapon>();
        PickArtifact?.Invoke(this, new ArtifactArgs(weapon.Name, weapon.Description));
    }
}
