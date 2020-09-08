using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<GameObject> passages;
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
                    if (listRoom[index] == null)
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
    }
}
