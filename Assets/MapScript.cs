using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScript : MonoBehaviour
{
    public int roomSize;
    public int passageSize;
    public bool[] roomShowed;
    public bool[] passageShowed;
    public List<GameObject> roomInstances;
    public enum MapStatus { Create, Update}
    public MapStatus mapStatus;
    private GameObject roomMapPrefab;
    private GameObject passageMapPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void UpdateMap(List<Room> rooms, List<Passage> passages, int currentRoomIndex)
    {
        if(mapStatus == MapStatus.Create)
        {
            roomMapPrefab = Resources.Load<GameObject>("Prefabs/Map/RoomMap");
            passageMapPrefab = Resources.Load<GameObject>("Prefabs/Map/PassageMap");
            mapStatus = MapStatus.Update;
            roomShowed = new bool[rooms.Count];
            passageShowed = new bool[passages.Count];
            roomInstances = new List<GameObject>();
            for (int i = 0; i < rooms.Count; i++)
                roomInstances.Add(null);
            roomInstances[0] = Instantiate(roomMapPrefab, transform);
            roomInstances[0].transform.localPosition = new Vector3(0, 0, 0);
            roomInstances[0].GetComponent<Image>().color = new Color(0, 1, 1, 1);
            roomShowed[0] = true;
        }
        for(int i = 1; i < rooms.Count; i++)
            if(rooms[i].visible)
            {
                if(!roomShowed[i])
                {
                    roomShowed[i] = true;
                    roomInstances[i] = Instantiate(roomMapPrefab, transform);
                    roomInstances[i].transform.localPosition = new Vector3(rooms[i].x * (roomSize + passageSize), rooms[i].y * (roomSize + passageSize), 0);
                }
                if(i == currentRoomIndex)
                    roomInstances[i].GetComponent<Image>().color = new Color(0, 1, 1, 1);
                else if (rooms[i].isCleared)
                    roomInstances[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        for(int i = 0; i < passages.Count; i++)
            if(!passageShowed[i] && passages[i].visible)
            {
                passageShowed[i] = true;
                GameObject obj = Instantiate(passageMapPrefab, transform);
                obj.transform.localPosition = new Vector3(passages[i].x * (roomSize + passageSize), passages[i].y * (roomSize + passageSize), 0);
            }
    }
}
