using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType type;
}

public enum RoomType
{
    Normal,
    Spawn,
    Boss,
    Shop,
    Workshop
}
