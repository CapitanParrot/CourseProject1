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

    // NEW ������� ��� �������������
    public event EventHandler InitEvent;

    // NEW ������� ��� ����������� ����
    public event EventHandler<RestartArgs> RestartEvent;

    // NEW ������� ��� ������ �����
    public event EventHandler<int> NextLevelEvent;

    // NEW ������� ��� ������ ����
    public event EventHandler<int> FinishGameEvent;
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
        
        // NEW �������� ������� �������������
        InitEvent?.Invoke(this, new EventArgs());
        PlayerManager.Instance.PlayerDeathEvent += PlayerDeath;
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
        //UIManager.Instance.ActivateInsert(LevelCounter);
        NextLevelEvent?.Invoke(this, LevelCounter);
    }

    // ����� ����.
    public void FinishGame()
    {
        FinishGameEvent?.Invoke(this, DeathCounter);
        UIManager.Instance.ShowEndScreen();
    }

    // ���������� ����� ������.
    public void RestartGame()
    {
        Destroy(ArtifactManager.Instance.gameObject);
        Destroy(EnemyManager.Instance.gameObject);
        Destroy(PlayerInstance);
        LevelCounter = 0;

        RestartEvent?.Invoke(this, new RestartArgs());
        //UIManager.Instance.DeactivateBossHealth();
        Run();
    }

    public void PlayerDeath(object sender, PlayerDeathArgs e)
    {
        DeathCounter++;
    }
}
