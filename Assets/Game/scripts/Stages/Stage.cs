using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public List<Room> rooms;
    
    public int PackagesDelivered;
    public int PackagesNeedForComplite;

    public int maxRoomNumber;

    void Start()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            rooms[i].roomNumber = i+1;
        }
        foreach(Room room in rooms)
        {
            if (maxRoomNumber < room.roomNumber)
            {
                maxRoomNumber = room.roomNumber;
            }
        }
    }

}
