using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ”правл€ет врагами на этажах и в комнатах.
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    // —четчик врагов в комнате, каждый уважающий себ€ враг прибавл€ет 1 когда спавнитс€.
    private int enemyInRoomCounter = 0;
    public List<List<GameObject>> Enemies = new List<List<GameObject>>();

    public List<GameObject> FirstFloorEnemies;
    public List<GameObject> SecondFloorEnemies;
    public List<GameObject> ThirdFloorEnemies;

    // Ѕоссов нужно добавл€ть в пор€дке по€влени€ в игре.
    public List<GameObject> Bosses;

    public int GetEnemyCount()
    {
        return enemyInRoomCounter;
    }
    public void AddEnemy()
    {
        enemyInRoomCounter++;
    }

    // ≈сли враги в комнате кончились, заканчиваем битву.
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
