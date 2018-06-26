using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    public Canvas canv;
    private float currentTimeScale;
    private bool paused;

    public static ButtonController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Start()
    {
        paused = false;
        canv.enabled = false;
    }
    public void Resume()
    {
        if (!PlayerSettings.instance.isAlive)
        {
            return;
        }
        paused = false;
        Time.timeScale = currentTimeScale;
        canv.enabled = false;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        paused = false;
        Application.LoadLevel(0);
    }
    public void Exit()
    {
        
        Application.Quit();
    }
    public void PauseGame()
    {
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        canv.enabled = true;
        paused = true;
    }
    public void UnPauseGame()
    {
        
        Time.timeScale = currentTimeScale;
        canv.enabled = false;
        paused = false;
    }

    private void Update()
    {
        if (paused)
            Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.Escape)&&!paused)
        {
            
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)&&paused)
        {
            UnPauseGame();
        }
    }
}
