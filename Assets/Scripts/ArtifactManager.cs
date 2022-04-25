using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance;
    public List<GameObject> Weapons;

    public List<GameObject> Artifacts;

    public int WeaponChance;
    private System.Random rnd;

    void Awake()
    {
        rnd = GameManager.Instance.Rnd;
        Instance = this;

    }
    

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
