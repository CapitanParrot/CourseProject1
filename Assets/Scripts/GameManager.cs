using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = System.Random;
using Pathfinding;

// ���� ���� ����, ��������� ��� �������.
public class GameManager : MonoBehaviour
{
    public GameObject LevelCreatorPrefab;

    public GameObject LevelCreatorInstance;

    public GameObject PlayerPrefab;
    public GameObject PlayerInstance;

    public GameObject EnemyManagerPrefab;
    public GameObject ArtifactManagerPrefab;
    public Camera Cam;
    public CinemachineVirtualCamera VirtualCamera;
    public int LevelCounter = 0;

    // ��� ���� ��������� LevelCreator, ��� ������ ������� �����.
    public Grid Exit;

    public static GameManager Instance;

    public Random Rnd;

    public int DeathCounter = 0;

    public int EndLevel = 3;
    void Start()
    {
        Run();
    }

    void Awake()
    {
        Rnd = new Random(DateTime.Now.Millisecond);
        Instance = this;
    }

    // ��������� �����.
    void Run()
    {
        PlayerInstance = Instantiate(PlayerPrefab);
        PlayerMovement pm = PlayerInstance.GetComponent<PlayerMovement>();
        pm.cam = Cam;
        VirtualCamera.Follow = PlayerInstance.transform;
        Instantiate(EnemyManagerPrefab);
        Instantiate(ArtifactManagerPrefab);
        NextLevel();
    }

    // ������ ��������� �������.
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
        UIManager.Instance.ActivateInsert(LevelCounter);

    }

    // ����� ����.
    public void FinishGame()
    {
        UIManager.Instance.ShowEndScreen();
    }

    // ���������� ����� ������.
    public void RestartGame()
    {
        Destroy(ArtifactManager.Instance.gameObject);
        Destroy(EnemyManager.Instance.gameObject);
        Destroy(PlayerInstance);
        LevelCounter = 0;
        Run();
    }
}
