using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<GameObject> passages;
    private List<int[]> listPassagesConnectedToRoom = new List<int[]>() { new int[2] { 0, 2 }, new int[2] { 1, 4 }, new int[2] { 7, 10 }, new int[2] { 9, 11 } };
    private List<int> listRoomsCorner = new List<int>() { 0, 2, 6, 8 };
    // Start is called before the first frame update

    public void UpdateMinimap(List<Room> listRoom, List<bool> listPassage)
    {
        for(int i = 0; i < 3; i++)
            for(int j = 0; j < 3; j++)
            {
                int index = i * 3 + j;
                Image img = rooms[index].GetComponent<Image>();
                if (i == 1 && j == 1)
                    img.color = new Color(1, 1, 1, 1);
                else
                {
                    if (listRoom[index] == null || (!listRoom[index].isCleared && (index == 0 || index == 2 || index == 6 || index == 8)))
                        img.color = new Color(1, 1, 1, 0);
                    else if (listRoom[index].isCleared)
                        img.color = new Color(1, 1, 1, 1);
                    else
                        img.color = new Color(1, 1, 1, 0.392f);
                }
            }
        for(int i = 0; i < listPassage.Count; i++)
            if(listPassage[i])
                passages[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.392f);
            else
                passages[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        for(int i = 0; i < listRoomsCorner.Count; i++)
            if(listRoom[listRoomsCorner[i]] == null || !listRoom[listRoomsCorner[i]].isCleared)
                for(int j = 0; j < listPassagesConnectedToRoom[i].Length; j++)
                    passages[listPassagesConnectedToRoom[i][j]].GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
}
