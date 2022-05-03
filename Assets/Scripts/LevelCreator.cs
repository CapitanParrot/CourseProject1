using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public int Level = 1;
    public int xSize = 9;
    public int ySize = 8;
    public int xStart = 3;
    public int yStart = 4;
    public float RoomSize = 14f;
    public GameObject Room;

    public List<Grid> RoomDecors = new List<Grid>();
    public List<GameObject> Rooms = new List<GameObject>();

    public Grid StartRoomDecor;

    public Grid BossRoomDecor;

    public Grid SpawnPoint;

    public Grid Exit;

    public Grid GoldRoomDecor;


    void Start()
    {
        // Генерируем план.
        PlanGenerator PG = new PlanGenerator(Level, xSize, ySize, xStart, yStart, GameManager.Instance.Rnd);
        int a = 0;
        while (!PG.GeneratePlan())
        {
            a++;
        }

        // По тегам расставляем комнаты.
        PG.SetupTags();
        for (int i = 0; i < PG.plan.GetLength(0); i++)
        {
            for (int j = 0; j < PG.plan.GetLength(1); j++)
            {
                if (PG.plan[i, j] != null)
                {
                    GameObject currentRoom = Instantiate(FindRoom(PG.plan[i, j].Tag),
                        new Vector3(RoomSize * i, -RoomSize * j), Quaternion.identity, transform);
                    Room room = null;
                    for (int k = 0; k < currentRoom.transform.childCount; k++)
                    {
                        if (currentRoom.transform.GetChild(k).tag.Equals("Ground"))
                        {
                            room = currentRoom.transform.GetChild(k).GetComponent<Room>();
                        }
                    }
                    if (PG.plan[i, j].state == RoomStates.Start)
                    {
                        // Стартовая комната не боевая.
                        if(room != null)
                        {
                            room.Decor = Instantiate(StartRoomDecor, new Vector3(RoomSize * i, -RoomSize * j),
                                Quaternion.identity, transform);
                            room.IsClear = true;
                        }

                        Instantiate(SpawnPoint, new Vector3(RoomSize * i, -RoomSize * j), Quaternion.identity,
                            transform);
                    }
                    else if (PG.plan[i, j].state == RoomStates.Boss)
                    {
                        // В комнате босса должен быть выход
                        if (room != null)
                        {
                            room.isBossRoom = true;
                            room.Decor = Instantiate(BossRoomDecor, new Vector3(RoomSize * i, -RoomSize * j),
                                Quaternion.identity, transform);
                        }
                        GameManager.Instance.Exit = Instantiate(Exit, new Vector3(RoomSize * i, -RoomSize * j),
                            Quaternion.identity, transform);
                        GameManager.Instance.Exit.gameObject.SetActive(false);
                    }else if(PG.plan[i, j].state == RoomStates.Gold)
                    {
                        if (room != null)
                        {
                            room.IsClear = true;
                            print("Golden spawned");
                            room.Decor = Instantiate(GoldRoomDecor, new Vector3(RoomSize * i, -RoomSize * j),
                                Quaternion.identity, transform);
                        }
                    }
                    else
                    {
                        if(room != null)
                        {
                            print("Decor spawned");
                            room.Decor = Instantiate(RoomDecors[GameManager.Instance.Rnd.Next(RoomDecors.Count)],
                                new Vector3(RoomSize * i, -RoomSize * j), Quaternion.identity, transform);
                        }
                    }
                }
            }
        }
    }

    // Ищет нужную комнату по тегу.
    GameObject FindRoom(string tag)
    {
        foreach (var room in Rooms)
        {
            if (room.tag == tag)
            {
                return room;
            }
        }

        return new GameObject();
    }
}
