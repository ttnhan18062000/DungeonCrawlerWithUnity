using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCanvasScript : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    private void Awake()
    {
        pauseMenuUI = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused == true)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
