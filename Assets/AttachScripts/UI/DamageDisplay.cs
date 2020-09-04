using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    GameObject dpuPrefab;
    GameObject currentObj;
    public bool isPopup;
    public float timeDelayCount;
    float timeDelayMax;
    // Start is called before the first frame update
    void Start()
    {
        dpuPrefab = Resources.Load<GameObject>("Prefabs/UI/DamagePopUp");
        isPopup = false;
        timeDelayMax = 0.75f;
        timeDelayCount = 0f;
    }
    
    public void Display(int damage, int size)
    {
        if (isPopup == false)
        {
            isPopup = true;
            currentObj = Instantiate(dpuPrefab, transform);
            currentObj.GetComponent<Text>().text = damage.ToString();
            //currentObj.GetComponent<DamagePopUp>().maxSize = size;
            Invoke("endStartAnim", 0.05f);
        }
        else
        {
            currentObj.GetComponent<Text>().text = (System.Convert.ToInt32(currentObj.GetComponent<Text>().text) + damage).ToString();
            timeDelayCount = 0;
        }
    }

    void endStartAnim()
    {
        currentObj.GetComponent<Animator>().SetBool("isShaking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPopup == true)
        {
            timeDelayCount += Time.deltaTime;
            if (timeDelayCount > timeDelayMax)
            {
                timeDelayCount = 0;
                isPopup = false;
                currentObj.GetComponent<Animator>().SetBool("isEnded", true);
                Destroy(currentObj, 0.5f);
            }
        }
    }
}
