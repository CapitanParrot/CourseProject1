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

// Класс комнаты для генератора планов.
public class PlanRoom
{
    // Список тупиков.
    public static List<PlanRoom> DeadEndRooms = new List<PlanRoom>();
    
    // Координаты.
    public int x;
    public int y;

    public RoomStates state;

    // Тэг для расстановки входов.
    public string Tag = "";
    public PlanRoom(int x, int y)
    {
        state = RoomStates.Usual;
        this.x = x;
        this.y = y;
    }
}
