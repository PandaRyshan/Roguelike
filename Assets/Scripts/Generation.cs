using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public int mapWidth = 7;
    public int mapHeight = 7;
    public int roomsToGenerate = 12;

    private int roomCount;
    private bool roomsInstantiated;
    private Vector2 firstRoomPos;

    // 2D Array to save the positions of the rooms (maps)
    private bool[,] map;
    public GameObject roomPrefab;

    private List<Room> roomObjects = new List<Room>();

    public static Generation instance;

    public float enemySpawnChance;
    public float coiniSpawnChance;
    public float healthSpawnChance;
    public int maxEnemiesPerRoom;
    public int maxCoinsPerRoom;
    public int maxHealthPerRoom;

    void Awake()
    {
        instance = this;
    }

    // called at the start of the game - begins the generation process
    public void Generate()
    {
        map = new bool[mapWidth, mapHeight];
        CheckRoom(3, 3, 0, Vector2.zero, true);
        InstantiateRooms();
        // convert tile coordinates to globle coordinates
        FindObjectOfType<Player>().transform.position = firstRoomPos * 12;

        UI.instance.map.texture = MapTextureGenerator.Generate(map, firstRoomPos);
    }

    // checks to see if we can place a room here - ocntinues the branch in the general direction
    void CheckRoom(int x, int y, int remainingRooms, Vector2 generalDirection, bool firstRoom = false)
    {
        // return if we have generated enough rooms
        if (roomCount >= roomsToGenerate) {
            return;
        }
        // return if the room is outside of the map bounds
        if (x < 0 || x > mapWidth - 1 || y < 0 || y > mapHeight - 1) {
            return;
        }
        // return if we have got no remaining rooms
        if (firstRoom == false && remainingRooms <= 0) {
            return;
        }
        // return if this room already exists
        if (map[x, y] == true) {
            return;
        }
        // set the first room position
        if (firstRoom == true) {
            firstRoomPos = new Vector2(x, y);
        }
        roomCount++;
        map[x, y] = true;

        bool north = Random.value > (generalDirection == Vector2.up ? 0.2f : 0.8f);
        bool south = Random.value > (generalDirection == Vector2.down ? 0.2f : 0.8f);
        bool west = Random.value > (generalDirection == Vector2.left ? 0.2f : 0.8f);
        bool east = Random.value > (generalDirection == Vector2.right ? 0.2f : 0.8f);

        int maxRemainingRooms = roomsToGenerate / 4;

        if (north || firstRoom) {
            CheckRoom(
                x, y + 1,
                firstRoom ? maxRemainingRooms : remainingRooms - 1,
                firstRoom ? Vector2.up : generalDirection);
        }
        if (south || firstRoom) {
            CheckRoom(
                x, y - 1,
                firstRoom ? maxRemainingRooms : remainingRooms - 1,
                firstRoom ? Vector2.down : generalDirection);
        }
        if (west || firstRoom) {
            CheckRoom(
                x - 1, y,
                firstRoom ? maxRemainingRooms : remainingRooms - 1,
                firstRoom ? Vector2.left : generalDirection);
        }
        if (east || firstRoom) {
            CheckRoom(
                x + 1, y,
                firstRoom ? maxRemainingRooms : remainingRooms - 1,
                firstRoom ? Vector2.right : generalDirection);
        }
    }

    // once the rooms have been decided, they will be spawned in and setup
    void InstantiateRooms()
    {
        if (roomsInstantiated) {
            return;
        }

        roomsInstantiated = true;

        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                if (map[x, y] == false) {
                    continue;
                }

                GameObject roomObj = Instantiate(roomPrefab,
                                                 new Vector2(x, y) * 12, Quaternion.identity);
                Room room = roomObj.GetComponent<Room>();

                if (y < mapHeight - 1 && map[x, y + 1] == true) {
                    room.northDoor.gameObject.SetActive(true);
                    room.northWall.gameObject.SetActive(false);
                }
                if (y > 0 && map[x, y - 1] == true) {
                    room.southDoor.gameObject.SetActive(true);
                    room.southWall.gameObject.SetActive(false);
                }
                if (x > 0 && map[x - 1, y] == true) {
                    room.westDoor.gameObject.SetActive(true);
                    room.westWall.gameObject.SetActive(false);
                }
                if (x < mapWidth - 1 && map[x + 1, y] == true) {
                    room.eastDoor.gameObject.SetActive(true);
                    room.eastWall.gameObject.SetActive(false);
                }

                if (firstRoomPos != new Vector2(x, y)) {
                    room.GenerateInterior();
                }
                roomObjects.Add(room);
            }
        }

        CaculateKeyAndExit();
    }

    void CaculateKeyAndExit()
    {
        float maxDistance = 0;
        Room keySpawnRoom = null;
        Room exitDoorSpawnRoom = null;

        // find two rooms that have the furthest distance between them
        // and spawn the key in one and the exit door in the other
        foreach (Room roomA in roomObjects) {
            foreach (Room roomB in roomObjects) {
                if (roomA == roomB) {
                    continue;
                }

                float distance = Vector2.Distance(roomA.transform.position,
                                                  roomB.transform.position);

                if (distance > maxDistance) {
                    maxDistance = distance;
                    keySpawnRoom = roomA;
                    exitDoorSpawnRoom = roomB;
                }
            }
        }
        keySpawnRoom.SpawnPrefab(keySpawnRoom.keyPrefab);
        exitDoorSpawnRoom.SpawnPrefab(exitDoorSpawnRoom.exitDoorPrefab);
    }

    public void OnPlayerMove()
    {
        Vector2 playerPos = FindObjectOfType<Player>().transform.position;
        Vector2 roomPos = new Vector2(Mathf.RoundToInt(playerPos.x / 12),
                                      Mathf.RoundToInt(playerPos.y / 12));
        UI.instance.map.texture = MapTextureGenerator.Generate(map, roomPos);                  
    }

    // Start is called before the first frame update
    void Start()
    {
        int seed = 765;
        Random.InitState(seed);
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
