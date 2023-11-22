using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject baseRoom;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRooms(7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Room[] GenerateRooms(int roomAmount)
    {
        Room[,] roomMatrix = new Room[11, 11];
        Room[] rooms = new Room[roomAmount];

        Room room = gameObject.GetComponent<Room>();
        rooms[0] = room;
        roomMatrix[5, 5] = room;
        List<(int x, int y)> spots = new List<(int x, int y)>();
        spots.Add((4, 5));
        spots.Add((6, 5));
        spots.Add((5, 4));
        spots.Add((5, 6));


        for (int i = 1; i < roomAmount; i++)
        {
            bool spotFound = false;
            (int x, int y) spot = (5, 5);

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

            Vector3 spawnPos = new Vector3(transform.position.x + (spot.x - 5) * 30, transform.position.y, transform.position.z + (spot.y - 5) * 20);

            GameObject go = (GameObject)Instantiate(baseRoom, spawnPos, Quaternion.identity, transform);
            room = go.GetComponent<Room>();
            rooms[i] = room;
            roomMatrix[spot.x, spot.y] = room;

        }

        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

        return rooms;
    }

}
