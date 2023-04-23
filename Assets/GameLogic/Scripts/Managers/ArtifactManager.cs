using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using Random = System.Random;

// ���������� ���������.
public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance;
    
    public List<GameObject> Weapons;
    public List<GameObject> Artifacts;

    // ����, ��� ������� ������, � �� �������.
    public int WeaponChance;
    private System.Random rnd;

    public AudioSource AudioSource;
    public AudioClip PickupSound;

    public List<GameObject> Inventory = new List<GameObject>();

    // NEW ������� ������� ���������
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

    // ����������� ���� ��� ���������� ��������.
    public void PlaySound()
    {
        AudioSource.PlayOneShot(PickupSound);
    }
    

    // ���������� ��������, ���� ������, ���� �������.
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
    /// ����� �������� �� �������� �������� ������ �� ���� ����������, ������ ���� �����
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
    /// ����� ������ �� �������� �������� ������ �� ���� ����������, ������ ���� �����
    /// </summary>
    /// <param name="weaponObject"></param>
    public void PlayWeapon(GameObject weaponObject)
    {
        PlaySound();
        IWeapon weapon = weaponObject.GetComponent<IWeapon>();
        PickArtifact?.Invoke(this, new ArtifactArgs(weapon.Name, weapon.Description));
    }
}
