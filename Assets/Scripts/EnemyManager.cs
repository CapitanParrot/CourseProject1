using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public int EnemyInRoomCounter = 0;
    public List<GameObject> Enemies = new List<GameObject>();

    // Ѕоссов нужно добавл€ть в пор€дке по€влени€ в игре.
    public List<GameObject> Bosses;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddEnemy()
    {
        EnemyInRoomCounter++;
    }

    public void SubtractEnemy()
    {
        EnemyInRoomCounter--;
        if (EnemyInRoomCounter <= 0)
        {
            Room.CurrentRoom.FinishFight();
        }
    }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
