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

    // Start is called before the first frame update
    void Start()
    {
        PlanGenerator PG = new PlanGenerator(Level, xSize, ySize, xStart, yStart,GameManager.Instance.Rnd);
        int a = 0;
        while (!PG.GeneratePlan())
        {
            a++;
        }
        PG.SetupTags();
        for (int i = 0; i < PG.plan.GetLength(0); i++)
        {
            for (int j = 0; j < PG.plan.GetLength(1); j++)
            {
                if (PG.plan[i, j] != null)
                {
                    GameObject currentRoom = Instantiate(FindRoom(PG.plan[i, j].Tag), new Vector3(RoomSize * i, -RoomSize * j), Quaternion.identity, transform);
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
                        if(room != null)
                        {
                            room.Decor = Instantiate(StartRoomDecor, new Vector3(RoomSize * i, -RoomSize * j),
                                Quaternion.identity, transform);
                            room.IsClear = true;
                        }
                        Instantiate(SpawnPoint, new Vector3(RoomSize * i, -RoomSize * j), Quaternion.identity, transform);
                    }
                    else if (PG.plan[i, j].state == RoomStates.Boss)
                    {
                        if (room != null)
                        {
                            room.isBossRoom = true;
                            room.Decor = Instantiate(BossRoomDecor, new Vector3(RoomSize * i, -RoomSize * j),
                                Quaternion.identity, transform);
                        }
                        GameManager.Instance.Exit = Instantiate(Exit, new Vector3(RoomSize * i, -RoomSize * j),
                            Quaternion.identity, transform);
                        GameManager.Instance.Exit.gameObject.SetActive(false);
                    }
                    else
                    {
                        //Room room = null;
                        //for (int k = 0; k < currentRoom.transform.childCount; k++)
                        //{
                            
                        //    if (currentRoom.transform.GetChild(k).tag.Equals("Ground"))
                        //    {
                        //        room = currentRoom.transform.GetChild(k).GetComponent<Room>();
                        //    }
                        //}
                        if(room != null)
                        {
                            print("Decor spawned");
                            room.Decor = Instantiate(RoomDecors[0], new Vector3(RoomSize * i, -RoomSize * j), Quaternion.identity, transform);
                        }
                        
                    }
                }
            }
        }
        //Instantiate(PlanRoom, new Vector3(14f, 0), Quaternion.identity, transform);
    }

    void Awake()
    {
        
    }

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
