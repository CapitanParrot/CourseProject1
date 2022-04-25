using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public GameObject LevelCreatorPrefab;

    public GameObject LevelCreatorInstance;

    public GameObject PlayerPrefab;
    public GameObject PlayerInstance;

    public GameObject EnemyManager;
    public GameObject ArtifactManager;
    public int LevelCounter = 0;


    public Grid Exit;
    // Start is called before the first frame update

    public static GameManager Instance;

    public Random Rnd;
    void Start()
    {
        Run();
    }

    void Awake()
    {
        Rnd = new Random(DateTime.Now.Millisecond);
        Instance = this;
        //PlayerInstance = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Run()
    {
        Instantiate(EnemyManager);
        Instantiate(ArtifactManager);
        NextLevel();
        
    }
    public void NextLevel()
    {
        LevelCounter++;
        if(LevelCreatorInstance != null)
        {
            Destroy(LevelCreatorInstance);
        }
        LevelCreator LC = LevelCreatorPrefab.GetComponent<LevelCreator>();
        LC.Level = LevelCounter;
        LevelCreatorInstance = Instantiate(LevelCreatorPrefab, Vector3.zero, Quaternion.identity);
    }
}
