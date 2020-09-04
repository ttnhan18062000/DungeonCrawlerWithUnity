using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public float currentTime;
    public float delaySpawn;
    public GameObject TheShy;

    public static bool isPaused = false;
    public static bool pauseMenuShowed = false;
    public static bool characterTabShowed = false;

    public GameObject pauseMenuUI;
    public GameObject characterTabUI;

    public GameObject CommonEnemy;
    public GameObject SniperEnemy;

    private void Awake()
    {
        pauseMenuUI = GameObject.FindGameObjectWithTag("PauseMenuUI");
        characterTabUI = GameObject.FindGameObjectWithTag("CharacterTabUI");
        pauseMenuUI.SetActive(false);
        characterTabUI.SetActive(false);
        CommonEnemy = Resources.Load<GameObject>("Prefabs/Enemies/Common/Common");
        SniperEnemy = Resources.Load<GameObject>("Prefabs/Enemies/Sniper/Sniper");
    }
    // Start is called before the first frame update
    void Start()
    {
        //TheShy = Resources.Load<GameObject>("Prefabs/TheShy");
        //Invoke("SpawnBoss", 3f);
    }

    void SpawnBoss()
    {
        GameObject go = Instantiate(TheShy);
    }


    // Update is called once per frame
    void Update()
    {
        
        if (currentTime <= 0)
        {
            int x = Random.Range(-2200, 2500);
            int y = Random.Range(-1000, 1500);
            Instantiate(CommonEnemy, new Vector3(x, y, 0), Quaternion.identity);
            //x = Random.Range(-2200, 2500);
            //y = Random.Range(-1000, 1500);
            //Instantiate(SniperEnemy, new Vector3(x, y, 0), Quaternion.identity);
            currentTime = delaySpawn;
        }
        else
            currentTime -= Time.deltaTime;
            
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
}
