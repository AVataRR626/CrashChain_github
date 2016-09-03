/*
QSI Utils

Matt Cabanag
*/

//general level loading utility.
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoadUtil : MonoBehaviour
{
    public bool loadNextOnStart = false;
    public bool reloadLevelOnStart = false;
    public float timer = 3;
    public string level = "next";
    public bool timerSkipClick = true;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(timerSkipClick)
        {
            if (Input.GetMouseButtonDown(0))
                timer = 0;
        }

        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        { 
            if (loadNextOnStart)
                LoadNextLevelI();

            if (reloadLevelOnStart)
                ReloadLevel();
        }
    }

    public void LoadNextLevelI()
    {
        //Debug.Log(level);

        if (level == "next")
            LoadNextLevel();
        else
            SceneManager.LoadScene(level);
    }
    
    public void RestartLevelI()
    {
        ReloadLevel();
    } 

    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
