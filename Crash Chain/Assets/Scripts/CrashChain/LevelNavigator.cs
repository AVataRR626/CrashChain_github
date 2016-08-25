﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelNavigator : MonoBehaviour
{
    [Header("Default Mode Settings")]
    public string puzzlePrefix = "puzzle_";
    public char delimiter = '_';
    public int setNumber = -1;
    public int puzzleNumber = -1;
    public bool autoSetBySceneName = true;
    

    [Header("Custom Mode Settings")]
    public bool customMode = false;
    public string customSetName = "";
    public string customPrefix = "lvl";
    public string customDelimiter = ":";

    // Use this for initialization
    void Start ()
    {
	    if(autoSetBySceneName)
        {
            string sceneString = SceneManager.GetActiveScene().name;

            string[] components = sceneString.Split(delimiter);

            int.TryParse(components[1], out setNumber);
            int.TryParse(components[2], out puzzleNumber);

            if(setNumber > 0)
                PlayerPrefs.SetInt("CurrentSet", setNumber);
            else
                PlayerPrefs.SetInt("CurrentSet", 1);
        }
        
        if(customMode)
        {   
            customSetName = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetKey);
            puzzleNumber = PlayerPrefs.GetInt(PuzzleLoader.currentCustomPuzzleNumberKey);
        }
	}
	
	void Update () {
	
	}

    public string GetSceneName()
    {
        if(!customMode)
        {
            return SceneManager.GetActiveScene().name;
        }
        else
        {
            return customSetName + customDelimiter + (puzzleNumber).ToString();
        }
    }
}