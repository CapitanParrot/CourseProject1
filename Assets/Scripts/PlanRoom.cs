using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomStates
{
    Usual,
    Gold,
    Boss,
    Start
}
public class PlanRoom
{
    public static List<PlanRoom> DeadEndRooms = new List<PlanRoom>();
    
    public int x;
    public int y;
    public RoomStates state;
    public string Tag = "";
    public PlanRoom(int x, int y)
    {
        state = RoomStates.Usual;
        this.x = x;
        this.y = y;
    }
}
