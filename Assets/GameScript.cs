using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Room
{
    public int x;
    public int y;
    public int id;
    public bool isCleared;
    public bool visible;
    public int distance;
    //public int distanceFromStart;
    public Room(int x, int y, int id)
    {
        this.x = x;
        this.y = y;
        this.id = id;
        this.isCleared = false;
        this.distance = -1;
        this.visible = false;
        //this.distanceFromStart = 0;
    }
}

public class Passage
{
    public float x;
    public float y;
    public bool visible;
    public Passage(float x, float y)
    {
        this.x = x;
        this.y = y;
        this.visible = visible;
    }
}

public class GameScript : MonoBehaviour
{
    public float currentTime;
    public float delaySpawn;
    public GameObject TheShy;

    public static bool isPaused = false;
    public static bool pauseMenuShowed = false;
    public static bool characterTabShowed = false;

    public int[] listEnemiesPower;

    public GameObject character;

    public GameObject pauseMenuUI;
    public GameObject characterTabUI;

    public GameObject CommonEnemy;
    public GameObject SniperEnemy;

    public GameObject door;

    public List<GameObject> roomPrefabs;
    private GameObject horizontalPassagePrefab;
    private GameObject verticalPassagePrefab;
    public List<GameObject> rooms;
    public List<Room> roomsInf;
    public List<Passage> passagesInf;
    public List<GameObject> passages;
    public List<Node> roomsNode;

    public int roomWidth;
    public int roomHeight;
    public int hPassageSize;
    public int vPassageSize;
    public int blockSize;

    public int roomLimit;

    public int currentFightingRoomIndex;

    public enum GameStatus { Idle, Battle};
    public GameStatus gameStatus;
    public List<GameObject> enemiesRemaining;
    public int clearedRoomAmount;

    public GameObject minimap;
    public GameObject mapCurrent;
    public GameObject map;
    public bool isDisplayMap;
    private int currentRoomIndex;
    private List<int[]> listPassageMinimap = new List<int[]>(12) { new int[2] {-2000, 3000}, new int[2] {2000, 3000},
    new int[2] {-4000, 1500}, new int[2] {0, 1500}, new int[2] {4000, 1500}, new int[2] {-2000, 0}, new int[2] {2000, 0},
    new int[2] {-4000, -1500}, new int[2] {0, -1500}, new int[2] {4000, -1500}, new int[2] {-2000, -3000}, new int[2] {2000, -3000}};

    private void Awake()
    {
        //pauseMenuUI = GameObject.FindGameObjectWithTag("PauseMenuUI");
        //characterTabUI = GameObject.FindGameObjectWithTag("CharacterTabUI");
        //pauseMenuUI.SetActive(false);
        //characterTabUI.SetActive(false);
        CommonEnemy = Resources.Load<GameObject>("Prefabs/Enemies/Common/Common");
        SniperEnemy = Resources.Load<GameObject>("Prefabs/Enemies/Sniper/Sniper");
        roomPrefabs = new List<GameObject>();
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/LRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/RRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TLRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TRRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BLRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BRRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/LRRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBLRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBRRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TLRRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BLRRoom"));
        roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBLRRoom"));
        horizontalPassagePrefab = Resources.Load<GameObject>("Prefabs/Map/HorizontalPassage");
        verticalPassagePrefab = Resources.Load<GameObject>("Prefabs/Map/VerticalPassage");
        character = GameObject.FindGameObjectWithTag("Character");
    }
    // Start is called before the first frame update
    void Start()
    {
        //TheShy = Resources.Load<GameObject>("Prefabs/TheShy");
        //Invoke("SpawnBoss", 3f);
        MapGenerator();
        gameStatus = GameStatus.Idle;
        clearedRoomAmount = 1;
        listEnemiesPower = new int[1] { 2};
        currentRoomIndex = -1;
        isDisplayMap = false;
    }

    void SpawnBoss()
    {
        GameObject go = Instantiate(TheShy);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(isDisplayMap)
            {
                map.SetActive(false);
                isDisplayMap = false;
            }
            else
            {
                map.SetActive(true);
                isDisplayMap = true;
                mapCurrent.GetComponent<MapScript>().UpdateMap(roomsInf, passagesInf, currentRoomIndex);
            }
        }
        if (gameStatus == GameStatus.Idle)
        {
            for (int i = 0; i < roomsInf.Count; i++)
            {
                Vector3 posRoom = rooms[i].transform.position;
                Vector3 posChar = character.transform.position;
                if (posChar.x <= posRoom.x + roomWidth / 2 && posChar.x >= posRoom.x - roomWidth / 2 && posChar.y <= posRoom.y + roomHeight / 2 && posChar.y >= posRoom.y - roomHeight / 2)
                {
                    //Update Minimap
                    if (currentRoomIndex != i)
                    {
                        currentRoomIndex = i;
                        int pX = roomsInf[i].x;
                        int pY = roomsInf[i].y;
                        List<Room> listRooms = new List<Room>();
                        for (int u = 1; u >= -1; u--)
                            for (int v = -1; v <= 1; v++)
                                if (roomsInf.Any(r => r.x == pX + v && r.y == pY + u))
                                    listRooms.Add(roomsInf.First(r => r.x == pX + v && r.y == pY + u));
                                else
                                    listRooms.Add(null);
                        List<bool> listPsg = new List<bool>();
                        for (int j = 0; j < listPassageMinimap.Count; j++)
                            if (passages.Any(p => p.transform.position.x == posRoom.x + listPassageMinimap[j][0] && p.transform.position.y == posRoom.y + listPassageMinimap[j][1]))
                            {
                                listPsg.Add(true);
                                //passagesInf[passages.IndexOf(passages.First(p => p.transform.position.x == posRoom.x + listPassageMinimap[j][0] && p.transform.position.y == posRoom.y + listPassageMinimap[j][1]))].visible = true;
                            }
                            else
                                listPsg.Add(false);
                        if (!roomsInf[i].visible)
                        {
                            roomsInf[i].visible = true;
                            try
                            {
                                passagesInf.First(p => p.x == pX + 0.5f && p.y == pY).visible = true;
                            }
                            catch (System.Exception e) { }
                            try
                            {
                                passagesInf.First(p => p.x == pX - 0.5f && p.y == pY).visible = true;
                            }
                            catch (System.Exception e) { }
                            try
                            {
                                passagesInf.First(p => p.x == pX && p.y == pY + 0.5f).visible = true;
                            }
                            catch (System.Exception e) { }
                            try
                            {
                                passagesInf.First(p => p.x == pX && p.y == pY - 0.5f).visible = true;
                            }
                            catch (System.Exception e) { }
                        }
                        minimap.GetComponent<MinimapScript>().UpdateMinimap(listRooms, listPsg);
                    }
                    //Check state of room
                    if (!roomsInf[i].isCleared)
                    {
                        gameStatus = GameStatus.Battle;
                        for (int j = 2; j < rooms[i].transform.childCount; j++)
                            rooms[i].transform.GetChild(j).GetComponent<DoorScript>().Close();
                        int roomPower = roomsInf[i].distance * 4;
                        while (roomPower >= listEnemiesPower.Min())
                        {
                            int x = Random.Range(Mathf.RoundToInt(posRoom.x - roomWidth / 2 + 300), Mathf.RoundToInt(posRoom.x + roomWidth / 2 - 300));
                            int y = Random.Range(Mathf.RoundToInt(posRoom.y - roomHeight / 2 + 300), Mathf.RoundToInt(posRoom.y + roomHeight / 2 - 300));
                            int enemiesId = 0;
                            do
                            {
                                enemiesId = Random.Range(0, listEnemiesPower.Length);
                            } while (roomPower < listEnemiesPower[enemiesId]);
                            if (enemiesId == 0)
                                enemiesRemaining.Add(Instantiate(CommonEnemy, new Vector3(x, y, 0), Quaternion.identity));
                            else if (enemiesId == 1)
                                enemiesRemaining.Add(Instantiate(SniperEnemy, new Vector3(x, y, 0), Quaternion.identity));
                            roomPower -= listEnemiesPower[enemiesId];
                        }
                        currentFightingRoomIndex = i;
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = enemiesRemaining.Count - 1; i >= 0; i--)
                if (enemiesRemaining[i] == null)
                    enemiesRemaining.RemoveAt(i);
            if(enemiesRemaining.Count == 0)
            {
                clearedRoomAmount++;
                gameStatus = GameStatus.Idle;
                roomsInf[currentFightingRoomIndex].isCleared = true;
                for (int i = 2; i < rooms[currentFightingRoomIndex].transform.childCount; i++)
                    rooms[currentFightingRoomIndex].transform.GetChild(i).GetComponent<DoorScript>().Open();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenuShowed = pauseMenuShowed == false;
                pauseMenuUI.SetActive(pauseMenuShowed);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                characterTabShowed = characterTabShowed == false;
                characterTabUI.SetActive(characterTabShowed);
                if(characterTabShowed == true)
                    GameObject.FindGameObjectWithTag("CharacterTabUI").GetComponent<CharacterTabScript>().ReloadCharacterStats();
            }
            Refresh();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        isPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        isPaused = true;
    }

    public void Refresh()
    {
        if (pauseMenuShowed == false && characterTabShowed == false)
            Resume();
        else
            Pause();
    }

    public void MapGenerator()
    {
        List<int> topRooms = new List<int>(7) { 4, 5, 6, 10, 11, 12, 14 };
        List<int> bottomRooms = new List<int>(7) { 4, 7, 8, 10, 11, 13, 14 };
        List<int> leftRooms = new List<int>(7) { 5, 7, 9, 10, 12, 13, 14 };
        List<int> rightRooms = new List<int>(7) { 6, 8, 9, 11, 12, 13, 14 };
        List<List<int>> roomNextPos = new List<List<int>>();
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TRoom"));0
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BRoom"));1
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/LRoom"));2
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/RRoom"));3
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBRoom"));4
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TLRoom"));5
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TRRoom"));6
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BLRoom"));7
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BRRoom"));8
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/LRRoom"));9
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBLRoom"));10
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBRRoom"));11
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TLRRoom"));12
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/BLRRoom"));13
        //roomPrefabs.Add(Resources.Load<GameObject>("Prefabs/Map/TBLRRoom"));14
        //0: top, 1: bottom, 2:left, 3:right
        roomNextPos.Add(new List<int>(1) { 1 });
        roomNextPos.Add(new List<int>(1) { 0 });
        roomNextPos.Add(new List<int>(1) { 3 });
        roomNextPos.Add(new List<int>(1) { 2 });
        roomNextPos.Add(new List<int>(2) { 1, 0 });
        roomNextPos.Add(new List<int>(2) { 1, 3 });
        roomNextPos.Add(new List<int>(2) { 1, 2 });
        roomNextPos.Add(new List<int>(2) { 0, 3 });
        roomNextPos.Add(new List<int>(2) { 0, 2 });
        roomNextPos.Add(new List<int>(2) { 3, 2 });
        roomNextPos.Add(new List<int>(3) { 1, 0, 3 });
        roomNextPos.Add(new List<int>(3) { 1, 0, 2 });
        roomNextPos.Add(new List<int>(3) { 1, 3, 2 });
        roomNextPos.Add(new List<int>(3) { 0, 3, 2 });
        roomNextPos.Add(new List<int>(3) { 1, 0, 3, 2 });
        List<int[]> nextPosAddition = new List<int[]>(4) { new int[2] { 0, -1 }, new int[2] { 0, 1 }, new int[2] { 1, 0 }, new int[2] { -1, 0 } };
        Queue<int> nextRoomQueue = new Queue<int>();
        Queue<int[]> previousRoomPosQueue = new Queue<int[]>();
        List<int[]> existedRoomPos = new List<int[]>();
        List<int[]> connectedRoomsDirection = new List<int[]>();
        roomsNode = new List<Node>();
        roomsInf = new List<Room>();
        passagesInf = new List<Passage>();
        roomsInf.Add(new Room(0, 0, 14));
        roomsInf[0].isCleared = true;
        existedRoomPos.Add(new int[2] { 0, 0 });
        for (int i = 0; i < roomNextPos[14].Count; i++)
        {
            nextRoomQueue.Enqueue(roomNextPos[14][i]);
            previousRoomPosQueue.Enqueue(new int[2] { 0, 0 });
        }
        while(nextRoomQueue.Count != 0)
        {
            int nextRoomPos = nextRoomQueue.Dequeue();
            int[] previousRoomPos = previousRoomPosQueue.Dequeue();
            int posX = previousRoomPos[0] + nextPosAddition[nextRoomPos][0];
            int posY = previousRoomPos[1] + nextPosAddition[nextRoomPos][1];
            if (existedRoomPos.Any(p => p[0] == posX && p[1] == posY))
            {
                continue;
            }
            int randomRoomID;
            if (nextRoomPos == 0)
            {
                if (Mathf.Abs(posX) + Mathf.Abs(posY) > roomLimit)
                    randomRoomID = 0;
                else
                    randomRoomID = topRooms[Random.Range(0, topRooms.Count)];
            }
            else if (nextRoomPos == 1)
            {
                if (Mathf.Abs(posX) + Mathf.Abs(posY) > roomLimit)
                    randomRoomID = 1;
                else
                    randomRoomID = bottomRooms[Random.Range(0, bottomRooms.Count)];
            }
            else if (nextRoomPos == 2)
            {
                if (Mathf.Abs(posX) + Mathf.Abs(posY) > roomLimit)
                    randomRoomID = 2;
                else
                    randomRoomID = leftRooms[Random.Range(0, leftRooms.Count)];
            }
            else
            {
                if (Mathf.Abs(posX) + Mathf.Abs(posY) > roomLimit)
                    randomRoomID = 3;
                else
                    randomRoomID = rightRooms[Random.Range(0, rightRooms.Count)];
            }
            GameObject psg;
            if (nextRoomPos == 0)
            {
                psg = Instantiate(verticalPassagePrefab);
                psg.transform.position = new Vector3(posX * (roomWidth + hPassageSize + blockSize), posY * (roomHeight + vPassageSize + blockSize) + (roomHeight + blockSize + vPassageSize)/2 , 0);
                passagesInf.Add(new Passage(posX, posY + 0.5f));
            }    
            else if (nextRoomPos == 1)
            {
                psg = Instantiate(verticalPassagePrefab);
                psg.transform.position = new Vector3(posX * (roomWidth + hPassageSize + blockSize), posY * (roomHeight + vPassageSize + blockSize) - (roomHeight + blockSize + vPassageSize) / 2, 0);
                passagesInf.Add(new Passage(posX, posY - 0.5f));
            }
            else if (nextRoomPos == 2)
            {
                psg = Instantiate(horizontalPassagePrefab);
                psg.transform.position = new Vector3(posX * (roomWidth + hPassageSize + blockSize) - (roomWidth + blockSize + hPassageSize) / 2, posY * (roomHeight + vPassageSize + blockSize), 0);
                passagesInf.Add(new Passage(posX - 0.5f, posY));
            }
            else
            {
                psg = Instantiate(horizontalPassagePrefab);
                psg.transform.position = new Vector3(posX * (roomWidth + hPassageSize + blockSize) + (roomWidth + blockSize + hPassageSize) / 2, posY * (roomHeight + vPassageSize + blockSize), 0);
                passagesInf.Add(new Passage(posX + 0.5f, posY));
            }
            passages.Add(psg);
            //GameObject obj = Instantiate(roomPrefabs[randomRoomID]);
            //obj.transform.position = new Vector3(posX * (roomWidth + hPassageSize + blockSize), posY * (roomHeight + vPassageSize + blockSize), 0);
            //rooms.Add(obj);
            existedRoomPos.Add(new int[2] { posX, posY });
            roomsInf.Add(new Room(posX, posY, randomRoomID));
            int[] connectDirection = new int[4] { 0, 0, 0, 0 };
            for(int i = 0; i < roomNextPos[randomRoomID].Count; i++)
            {
                nextRoomQueue.Enqueue(roomNextPos[randomRoomID][i]);
                previousRoomPosQueue.Enqueue(new int[2] { posX, posY});
            }
        }
        for (int i = 0; i < roomsInf.Count; i++)
            roomsNode.Add(new Node(i));
        for(int i = 0; i < roomsInf.Count; i++)
        {
            int roomID = roomsInf[i].id;
            int passageX = roomsInf[i].x * (roomWidth + hPassageSize + blockSize);
            int passageY = roomsInf[i].y * (roomHeight + vPassageSize + blockSize);
            int[] gates = new int[4] { 0, 0, 0, 0 };
            if (passages.Any(p => p.transform.position.x == passageX && p.transform.position.y == passageY - (roomHeight + blockSize + vPassageSize) / 2))
                gates[0]++;
            if (passages.Any(p => p.transform.position.x == passageX && p.transform.position.y == passageY + (roomHeight + blockSize + vPassageSize) / 2))
                gates[1]++;
            if (passages.Any(p => p.transform.position.x == passageX + (roomWidth + blockSize + hPassageSize) / 2 && p.transform.position.y == passageY))
                gates[2]++;
            if (passages.Any(p => p.transform.position.x == passageX - (roomWidth + blockSize + hPassageSize) / 2 && p.transform.position.y == passageY))
                gates[3]++;
            if(roomNextPos[roomID].Any(pos => gates[pos] == 0))
            {
                for(int j = 0; j <= 14; j++)
                {
                    int[] tmp = new int[4] { gates[0], gates[1], gates[2], gates[3] };
                    for (int k = 0; k < roomNextPos[j].Count; k++)
                        tmp[roomNextPos[j][k]]--;
                    if(tmp.All(val => val == 0))
                    {
                        roomsInf[i].id = j;
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < roomsInf.Count; i++)
        {
            int pX = roomsInf[i].x;
            int pY = roomsInf[i].y;
            for (int j = 0; j < roomNextPos[roomsInf[i].id].Count; j++)
            {
                if (roomNextPos[roomsInf[i].id][j] == 0)
                {
                    int id = roomsInf.IndexOf(roomsInf.First(r => r.x == pX && r.y == pY - 1));
                    roomsNode[i].next.Add(roomsNode.First(r => r.id == id));
                }
                else if (roomNextPos[roomsInf[i].id][j] == 1)
                {
                    int id = roomsInf.IndexOf(roomsInf.First(r => r.x == pX && r.y == pY + 1));
                    roomsNode[i].next.Add(roomsNode.First(r => r.id == id));
                }
                else if (roomNextPos[roomsInf[i].id][j] == 2)
                {
                    int id = roomsInf.IndexOf(roomsInf.First(r => r.x == pX + 1 && r.y == pY));
                    roomsNode[i].next.Add(roomsNode.First(r => r.id == id));
                }
                else if (roomNextPos[roomsInf[i].id][j] == 3)
                {
                    int id = roomsInf.IndexOf(roomsInf.First(r => r.x == pX - 1 && r.y == pY));
                    roomsNode[i].next.Add(roomsNode.First(r => r.id == id));
                }
            }
        }
        Node startNode = roomsNode[0];
        for (int i = 0; i < roomsNode.Count; i++)
        {
            roomsInf[i].distance = Utilities.GetDistanceBetweenTwoNodeWithBFS(startNode, i);
        }
        for (int i = 0; i < roomsInf.Count; i++)
        {
            GameObject obj = Instantiate(roomPrefabs[roomsInf[i].id]);
            obj.transform.position = new Vector3(roomsInf[i].x * (roomWidth + hPassageSize + blockSize), roomsInf[i].y * (roomHeight + vPassageSize + blockSize), 0);
            for (int j = 2; j < obj.transform.childCount; j++)
                obj.transform.GetChild(j).GetComponent<DoorScript>().Open();
            rooms.Add(obj);
            rooms[i].name = i.ToString() + "_" + roomsInf[i].distance.ToString();
        }
    }
}
