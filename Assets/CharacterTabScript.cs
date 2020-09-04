using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterTabScript : MonoBehaviour
{
    public GameObject weaponListCurrent;

    public GameObject attributeListCurrent;

    public GameObject statsListCurrent;

    private List<Vector2> limitScroll = new List<Vector2>(3) { new Vector2(), new Vector2(), new Vector2()}; //v.x : Bottom, v.y : Top

    private List<string> limitScrollNameDefine = new List<string>(3) { "WeaponList", "AttributeList", "StatsList"};

    public CharacterInventory inventory;
    public CharacterStatus status;

    public string currentTab;

    public static List<string> tabs;

    public bool IsAddAttributeEnabled = false;

    //public GameObject unspendAttributePoint; = transform.GetChild(2).GetChild(2).GetChild(2).GetChild(0).GetChild(1)

    private void Awake()
    {
        //weaponListCurrent = gameObject.transform.GetChild(4).GetChild(2).GetChild(0).gameObject;
        //attributeListCurrent = gameObject.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject;
        //statsListCurrent = gameObject.transform.GetChild(2).GetChild(2).GetChild(3).GetChild(1).GetChild(0).gameObject;
        inventory = GameObject.FindGameObjectWithTag("Character").gameObject.GetComponent<CharacterInventory>();
        status = GameObject.FindGameObjectWithTag("Character").gameObject.GetComponent<CharacterStatus>();
        tabs = new List<string>() { "Character", "Inventory", "Weapon" };
        LoadCharacterTab();
        LoadListWeapon();
        currentTab = "Character";
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        //gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(TabChangeClick);
        //gameObject.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(TabChangeClick);
        //gameObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(TabChangeClick);

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float msw = Input.GetAxis("Mouse ScrollWheel");
        if (msw != 0)
        {
            if (currentTab == "Character")
            {
                ScrollTab(attributeListCurrent, "AttributeList");
                ScrollTab(statsListCurrent, "StatsList");
            }
            else if (currentTab == "Weapon")
            {
                ScrollTab(weaponListCurrent, "WeaponList");
            }
        }
    }

    public void ScrollTab(GameObject scrollObj, string name)
    {
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        RectTransform rect = scrollObj.GetComponent<RectTransform>();
        RectTransform rectParent = scrollObj.transform.parent.GetComponent<RectTransform>();
        float rectBottom = rectParent.transform.position.y - rectParent.sizeDelta.y / 2;
        float rectTop = rectParent.transform.position.y + rectParent.sizeDelta.y / 2;
        float rectLeft = rectParent.transform.position.x - rectParent.sizeDelta.x / 2;
        float rectRight = rectParent.transform.position.x + rectParent.sizeDelta.x / 2;
        if (x >= rectLeft && x <= rectRight && y >= rectBottom && y <= rectTop)
        {
            float msw = Input.GetAxis("Mouse ScrollWheel");
            rect.localPosition += new Vector3(0, -msw * 300f, 0);
            if (rect.localPosition.y > limitScroll[limitScrollNameDefine.IndexOf(name)].x)
                rect.localPosition = new Vector3(0, limitScroll[limitScrollNameDefine.IndexOf(name)].x, 0);
            if (rect.localPosition.y < limitScroll[limitScrollNameDefine.IndexOf(name)].y)
                rect.localPosition = new Vector3(0, limitScroll[limitScrollNameDefine.IndexOf(name)].y, 0);
        }
    }

    public void TabChangeClick()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        gameObject.transform.GetChild(2 + tabs.IndexOf(currentTab)).gameObject.SetActive(false);
        gameObject.transform.GetChild(2 + tabs.IndexOf(name)).gameObject.SetActive(true);
        currentTab = name;
            
    }

    public void LoadAttribute()
    {
        GameObject attributeList = gameObject.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject;

        List<Attribute> attributes = new List<Attribute>(status.character.listOfAttribute);
        int num = attributes.Count;
        GameObject attributePrefab = Resources.Load<GameObject>("Prefabs/UI/Attribute");

        float attributeListSizeY = 120f * num;
        float attributeListLocationY = (480f - attributeListSizeY) / 2;

        attributeList.GetComponent<RectTransform>().sizeDelta = new Vector2(546, attributeListSizeY);
        attributeList.GetComponent<RectTransform>().localPosition = new Vector3(0, attributeListLocationY, 0);

        limitScroll[limitScrollNameDefine.IndexOf("AttributeList")] = new Vector2((attributeListSizeY - 480f) / 2,attributeListLocationY);
 

        for(int i = 0; i < attributes.Count; i++)
        {
            GameObject atr = Instantiate(attributePrefab, Vector3.zero, Quaternion.identity);
            RectTransform rect = atr.GetComponent<RectTransform>();
            rect.SetParent(attributeList.transform);
            rect.localPosition = new Vector3(0, (attributeListSizeY-120f-240f*i)/2, 0);

            atr.transform.GetChild(1).GetComponent<Text>().text = attributes[i].type;

            atr.transform.GetChild(2).GetComponent<Text>().text = attributes[i].value.ToString();
            atr.name = attributes[i].type;

            GameObject btn = atr.transform.GetChild(3).gameObject;
            btn.name = attributes[i].type;

            //btn.GetComponent<Button>().onClick.AddListener(AddPointToAttributeClick);
        }

        transform.GetChild(2).GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>().text = status.character.level.unspendAttributePoint.ToString();
        if (status.character.level.unspendAttributePoint > 0)
            SetAddPointToAttributeStatus(true);
        else
            SetAddPointToAttributeStatus(false);
    }

    public void AddPointToAttributeClick()
    {
        status.character.level.unspendAttributePoint--;
        if (status.character.level.unspendAttributePoint == 0)
        {
            GameObject.FindGameObjectWithTag("CharacterLevelUI").transform.GetChild(3).GetComponent<Image>().color = new Color32(255, 255, 0, 0);
            SetAddPointToAttributeStatus(false);
        }
        transform.GetChild(2).GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>().text = status.character.level.unspendAttributePoint.ToString();
        string name = EventSystem.current.currentSelectedGameObject.name;
        status.character.AddAttributes(name, 1);
        GameObject attributeList = GameObject.FindGameObjectWithTag("CharacterTabUI").transform.GetChild(2).GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject;
        for (int i = 0; i < attributeList.transform.childCount; i++)
        {
            if(attributeList.transform.GetChild(i).gameObject.name == name)
            {
                attributeList.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = status.character.GetAttribute(name).ToString();
                status.character.UpdateStats();
                ReloadStatsList();
                GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterStatus>().UpdateCharacterScaleAttribute();
                return;
            }
        }
    }

    public void SetAddPointToAttributeStatus(bool state)
    {
        //false: deactive, true: active
        GameObject atrList = transform.GetChild(2).GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject;
        if (state == false)
        {
            IsAddAttributeEnabled = false;
            for(int i = 0; i < atrList.transform.childCount; i++)
            {
                atrList.transform.GetChild(i).GetChild(3).GetComponent<Button>().onClick.RemoveListener(AddPointToAttributeClick);
                atrList.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            }
        }
        else
        {
            IsAddAttributeEnabled = true;
            for (int i = 0; i < atrList.transform.childCount; i++)
            {
                atrList.transform.GetChild(i).GetChild(3).GetComponent<Button>().onClick.AddListener(AddPointToAttributeClick);
                atrList.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }

    public void LoadStats()
    {
        GameObject statsList = gameObject.transform.GetChild(2).GetChild(2).GetChild(3).GetChild(1).GetChild(0).gameObject;
        List<Stats> stats = new List<Stats>(status.character.listOfStats);
        int num = stats.Count;
        GameObject statsPrefab = Resources.Load<GameObject>("Prefabs/UI/Stats");

        float statsListSizeY = 120f * num;
        float statsListLocationY = (480f - statsListSizeY) / 2;

        statsList.GetComponent<RectTransform>().sizeDelta = new Vector2(546, statsListSizeY);
        statsList.GetComponent<RectTransform>().localPosition = new Vector3(0, statsListLocationY, 0);

        limitScroll[limitScrollNameDefine.IndexOf("StatsList")] = new Vector2((statsListSizeY - 480f) / 2,statsListLocationY);

        for (int i = 0; i < stats.Count; i++)
        {
            GameObject sta = Instantiate(statsPrefab, Vector3.zero, Quaternion.identity);
            RectTransform rect = sta.GetComponent<RectTransform>();
            rect.SetParent(statsList.transform);
            rect.localPosition = new Vector3(0, (statsListSizeY - 120f - 240f * i) / 2, 0);

            sta.transform.GetChild(1).GetComponent<Text>().text = stats[i].type;

            sta.transform.GetChild(2).GetComponent<Text>().text = stats[i].value.ToString();
            sta.name = stats[i].type;
        }
    }

    public void ReloadStatsList()
    {
        List<Stats> stats = new List<Stats>(status.character.listOfStats);
        for(int i = 0; i < statsListCurrent.transform.childCount; i++)
        {
            statsListCurrent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = stats.FirstOrDefault(st => st.type == statsListCurrent.transform.GetChild(i).name).value.ToString();
        }
    }

    public void ReloadCharacterStats()
    {
        GameObject attributeList = transform.GetChild(2).GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject;
        GameObject statsList = transform.GetChild(2).GetChild(2).GetChild(3).GetChild(1).GetChild(0).gameObject;
        Character character = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterStatus>().character;
        for (int i = 0; i < attributeList.transform.childCount; i++)
        {
            string atrName = attributeList.transform.GetChild(i).name;
            attributeList.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = character.GetAttribute(atrName).ToString();
        }
        for (int i = 0; i < statsList.transform.childCount; i++)
        {
            string statsName = statsList.transform.GetChild(i).name;
            statsList.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = Utilities.ModifyFloatToOneDigit(character.GetStats(statsName)).ToString();
        }
        transform.GetChild(2).GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>().text = character.level.unspendAttributePoint.ToString();

        if (status.character.level.unspendAttributePoint > 0 && IsAddAttributeEnabled == false)
            SetAddPointToAttributeStatus(true);
    }

    public void LoadCharacterTab()
    {
        LoadAttribute();
        LoadStats();
    }

    public void LoadListWeapon()
    {
        GameObject weaponListDisplay = gameObject.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        List<Weapon> weapons = new List<Weapon>(inventory.listWeapon);
        int num = weapons.Count;
        GameObject btnPrefab = Resources.Load<GameObject>("Prefabs/UI/ButtonDisplayWeapon");

        float weaponListDisplaySizeY = 160f * num;
        float weaponListDisplayLocationY = (440f - weaponListDisplaySizeY) / 2;

        weaponListDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(465, weaponListDisplaySizeY);
        weaponListDisplay.GetComponent<RectTransform>().localPosition = new Vector3(0, weaponListDisplayLocationY, 0);

        limitScroll[limitScrollNameDefine.IndexOf("WeaponList")] = new Vector2((weaponListDisplaySizeY - 440f) / 2, weaponListDisplayLocationY);

        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject btn = Instantiate(btnPrefab, Vector3.zero, Quaternion.identity);
            Sprite sprite = Resources.Load<Sprite>("Images/Weapons/" + weapons[i].name + "Icon");
            btn.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;

            RectTransform rect = btn.GetComponent<RectTransform>();
            rect.SetParent(weaponListDisplay.transform);
            rect.localPosition = new Vector3(0, (weaponListDisplaySizeY-160f-320*i)/2, 0);

            btn.name = weapons[i].name;

            btn.GetComponent<Button>().onClick.AddListener(LoadWeaponOnClick);
        }
        LoadWeaponToDisplayTable(weapons[0].name);
    }



    private void LoadWeaponOnClick()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        LoadWeaponToDisplayTable(name);
    }

    private void LoadWeaponToDisplayTable(string name)
    {
        gameObject.transform.GetChild(4).gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Weapons/" + name + "Icon");
        Weapon weapon = inventory.listWeapon.FirstOrDefault(w => w.name == name);
        GameObject weaponStats = gameObject.transform.GetChild(4).gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject;
        weaponStats.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = weapon.damage.ToString();
        weaponStats.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = weapon.maxAmmo.ToString();
        weaponStats.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = weapon.reloadTime.ToString();
        weaponStats.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = (Mathf.Round(10f / weapon.delayShoot) / 10).ToString();
        weaponStats.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = weapon.acuracy.ToString();
        weaponStats.transform.GetChild(5).transform.GetChild(1).GetComponent<Text>().text = weapon.baseBulletSpeed.ToString();
        weaponStats.transform.GetChild(6).transform.GetChild(1).GetComponent<Text>().text = weapon.firingMode;
    }

    public void LoadWeaponTab()
    {
        LoadListWeapon();
    }
}
