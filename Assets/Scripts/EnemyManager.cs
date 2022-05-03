using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������� ������� �� ������ � � ��������.
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    // ������� ������ � �������, ������ ��������� ���� ���� ���������� 1 ����� ���������.
    private int enemyInRoomCounter = 0;
    public List<List<GameObject>> Enemies = new List<List<GameObject>>();

    public List<GameObject> FirstFloorEnemies;
    public List<GameObject> SecondFloorEnemies;
    public List<GameObject> ThirdFloorEnemies;

    // ������ ����� ��������� � ������� ��������� � ����.
    public List<GameObject> Bosses;

    public int GetEnemyCount()
    {
        return enemyInRoomCounter;
    }
    public void AddEnemy()
    {
        enemyInRoomCounter++;
    }

    // ���� ����� � ������� ���������, ����������� �����.
    public void SubtractEnemy()
    {
        enemyInRoomCounter--;
        if (enemyInRoomCounter <= 0)
        {
            Room.CurrentRoom.FinishFight();
        }
    }

    void Awake()
    {
        Instance = this;
        Enemies.Add(FirstFloorEnemies);
        Enemies.Add(SecondFloorEnemies);
        Enemies.Add(ThirdFloorEnemies);
    }
}
