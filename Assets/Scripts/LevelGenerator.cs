using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject baseRoom;
    [SerializeField] int roomAmount = 7;
    [SerializeField] GameObject[] normalRooms;

    // Start is called before the first frame update
    void Start()
    {
        Room[] rooms = GenerateRooms(roomAmount);

        rooms[0].type = RoomType.Spawn;
        rooms[^1].type = RoomType.Boss;

        rooms[Random.Range(1, rooms.Length - 1)].type = RoomType.Shop;

        for (int i = 1; i < rooms.Length; i++)
        {
            if (rooms[i].type == RoomType.Normal)
                Instantiate(normalRooms[0], rooms[i].transform);
        }

        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Room[] GenerateRooms(int roomAmount)
    {
        int mSize = 31;
        int frSpot = (int)(mSize / 2);
        Room[,] roomMatrix = new Room[mSize, mSize];
        Room[] rooms = new Room[roomAmount];

        Room room = gameObject.GetComponent<Room>();
        rooms[0] = room;
        roomMatrix[frSpot, frSpot] = room;
        List<(int x, int y)> spots = new List<(int x, int y)>
        {
            (frSpot - 1, frSpot),
            (frSpot + 1, frSpot),
            (frSpot, frSpot - 1),
            (frSpot, frSpot + 1)
        };


        for (int i = 1; i < roomAmount; i++)
        {
            bool spotFound = false;
            (int x, int y) spot = (frSpot, frSpot);

            while (!spotFound)
            {
                spot = spots[Random.Range(0, spots.Count)];
                if (roomMatrix[spot.x, spot.y] == null)
                {
                    spotFound = true;
                }
            }

            spots.Remove(spot);

            if (roomMatrix[spot.x - 1, spot.y] == null && !spots.Contains((spot.x - 1, spot.y))) { spots.Add((spot.x - 1, spot.y)); }
            if (roomMatrix[spot.x + 1, spot.y] == null && !spots.Contains((spot.x + 1, spot.y))) { spots.Add((spot.x + 1, spot.y)); }
            if (roomMatrix[spot.x, spot.y - 1] == null && !spots.Contains((spot.x, spot.y - 1))) { spots.Add((spot.x, spot.y - 1)); }
            if (roomMatrix[spot.x, spot.y + 1] == null && !spots.Contains((spot.x, spot.y + 1))) { spots.Add((spot.x, spot.y + 1)); }

            Vector3 spawnPos = new Vector3(transform.position.x + (spot.x - frSpot) * 30, transform.position.y, transform.position.z + (spot.y - frSpot) * 20);

            GameObject go = (GameObject)Instantiate(baseRoom, spawnPos, Quaternion.identity, transform);
            room = go.GetComponent<Room>();
            rooms[i] = room;
            roomMatrix[spot.x, spot.y] = room;

        }
        return rooms;
    }

}
