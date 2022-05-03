using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


// Генератов планов aka самый сложный класс.
// Создает абстактную карту для последующей расстановки графики.
// Супер удобно можно настроить размер и число нужных комнат.
public class PlanGenerator 
{
    // Пределы карты матрица 9 на 8.
    private static (int, int) mapSize = (9, 8);

    // С этой комнаты начинается генерация.
    private static (int, int) startRoomCoord = (3, 4);

    // Последовательность координат важна для генерации.
    private static List<(int, int)> surround = new List<(int, int)> { (-1, 0), (0, -1), (1, 0),(0, 1) };
    public PlanRoom[,] plan = new PlanRoom[mapSize.Item1, mapSize.Item2];
    public int Level = 1;

    // колличество комнат в генерации.
    private int RoomsNumber;
    private Random rnd;

    public PlanGenerator(int level, int xSize, int ySize, int xStart, int yStart, Random rnd)
    {
        this.rnd = rnd;
        Level = level;
        mapSize.Item1 = xSize;
        mapSize.Item2 = ySize;
        startRoomCoord.Item1 = xStart;
        startRoomCoord.Item2 = yStart;

        
        
    }

    // Шлавный метод генерации.
    public bool GeneratePlan()
    {
        // На первом эта же 7-8 комнат.
        RoomsNumber = (int)(rnd.Next(2) + 4 + Level * 2.6);

        // Генерация этажа начинается со стартовой комнаты.
        plan = new PlanRoom[mapSize.Item1, mapSize.Item2];
        PlanRoom startPlanRoom = new PlanRoom(startRoomCoord.Item1, startRoomCoord.Item2);
        startPlanRoom.state = RoomStates.Start;
        Queue<PlanRoom> planQueue = new Queue<PlanRoom>();
        plan[startRoomCoord.Item1, startRoomCoord.Item2] = startPlanRoom;
        planQueue.Enqueue(startPlanRoom);

        int currentRoomCounter = 0;
        PlanRoom.DeadEndRooms = new List<PlanRoom>();
        while (planQueue.Count != 0)
        {
            PlanRoom next = planQueue.Dequeue();
            int sidesCounter = 1;

            // Обходим 4 стороны комнаты.
            for (int i = 0; i < 4; i++)
            {
                // Проверка на выход из границ массива.
                if (next.x + surround[i].Item1 < 0
                    || next.x + surround[i].Item1 >= mapSize.Item1
                    || next.y + surround[i].Item2 < 0
                    || next.y + surround[i].Item2 >= mapSize.Item2)
                    continue;

                // Проверяю соседнюю клетку, если там не пусто - пропуск.
                if (plan[next.x + surround[i].Item1, next.y + surround[i].Item2] != null)
                {
                    continue;
                }

                // Условие генерации подземелья без циклов.
                if (CheckTunnel(next, i))
                {
                    continue;
                }

                // Комнат хватает.
                if (currentRoomCounter == RoomsNumber)
                {
                    continue;
                }

                // Просто ничего не делаем с вероятностью 50%.
                if (rnd.Next(2) == 0)
                {
                    continue;
                }

                PlanRoom newPlanRoom = new PlanRoom(next.x + surround[i].Item1, next.y + surround[i].Item2);
                plan[next.x + surround[i].Item1, next.y + surround[i].Item2] = newPlanRoom;
                planQueue.Enqueue(newPlanRoom);
                sidesCounter++;
                currentRoomCounter++;
            }

            if (sidesCounter == 1)
            {
                PlanRoom.DeadEndRooms.Add(next);
            }
        }
        if (PlanRoom.DeadEndRooms.Count < 3)
        {
            return false;
        }
        int randomIdx;

        // В общем случае комната с боссом будет самым дальним тупиком от старта.
        if (PlanRoom.DeadEndRooms[PlanRoom.DeadEndRooms.Count - 1].state == RoomStates.Usual)
        {
            PlanRoom.DeadEndRooms[PlanRoom.DeadEndRooms.Count - 1].state = RoomStates.Boss;
        }
        else
        {
            do
            {
                randomIdx = rnd.Next(PlanRoom.DeadEndRooms.Count - 1);
            } while (PlanRoom.DeadEndRooms[randomIdx].state != RoomStates.Usual);

            PlanRoom.DeadEndRooms[randomIdx].state = RoomStates.Boss;
        }
        do
        {
            randomIdx = rnd.Next(PlanRoom.DeadEndRooms.Count - 1);

        } while (PlanRoom.DeadEndRooms[randomIdx].state != RoomStates.Usual);

        PlanRoom.DeadEndRooms[randomIdx].state = RoomStates.Gold;

        // Вокруг стартовой комнаты не может сразу быть босса.
        for (int i = 0; i < 4; i++)
        {
            if (plan[startRoomCoord.Item1 + surround[i].Item1, startRoomCoord.Item2 + surround[i].Item2] != null &&
                plan[startRoomCoord.Item1 + surround[i].Item1, startRoomCoord.Item2 + surround[i].Item2].state == RoomStates.Boss)
            {
                return false;
            }
        }

        // Если не получилось нужное число комнат, начинаем сначала.
        if (currentRoomCounter != RoomsNumber)
        {
            return false;
        }
        return true;
    }

    // Проверка графа комнат на тунелеобразность.
    private bool CheckTunnel(PlanRoom next, int surrIdx)
    {
        int roomCounter = 0;
        for (int j = 0; j < 4; j++)
        {
            int newX = next.x + surround[surrIdx].Item1 + surround[j].Item1;
            int newY = next.y + surround[surrIdx].Item2 + surround[j].Item2;
            if (newX < 0
                || newX >= mapSize.Item1
                || newY < 0
                || newY >= mapSize.Item2)
            {
                continue;
            }

            if (plan[newX, newY] != null) roomCounter++;
        }

        if (roomCounter > 1) return true;
        return false;
    }
    
    // Расставляет флаги в комнтах.
    public void SetupTags()
    {
        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if(plan[i,j] == null)
                    {
                        break;
                    }

                    if (i + surround[k].Item1 < 0
                        || i + surround[k].Item1 >= mapSize.Item1
                        || j + surround[k].Item2 < 0
                        || j + surround[k].Item2 >= mapSize.Item2)
                    {
                        continue;
                    }

                    // Ставит метки входов
                    // L - слева есть комната 
                    // U - сверху
                    // R - справа
                    // D - снизу
                    if(plan[i + surround[k].Item1,j + surround[k].Item2] != null)
                    {
                        switch (k)
                        {
                            case 0:
                                plan[i, j].Tag += "L";
                                break;
                            case 1:
                                plan[i, j].Tag += "U";
                                break;
                            case 2:
                                plan[i, j].Tag += "R";
                                break;
                            case 3:
                                plan[i, j].Tag += "D";
                                break;
                        }
                    }
                }
            }
        }
    }
}
