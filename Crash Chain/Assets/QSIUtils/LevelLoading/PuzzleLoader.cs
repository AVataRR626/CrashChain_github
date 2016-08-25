using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleLoader : MonoBehaviour
{
    [Header("Default Mode Settings")]
    public string puzzlePrefix = "puzzle_";
    public string delimiter = "_";
    public int setNumber = 1;
    public int puzzleNumber = 1;

    [Header("Custom Mode Settings")]
    public bool customMode = false;
    public string customSetName = "";
    public string customPrefix = "lvl";
    public string customDelimiter = ":";
    public string customLevelLoader = "CustomLevelLoader";

    [Header("Lock Settings")]
    public bool checkLocked = true;
    public bool locked = false;
    public Color lockTint;

    public static string currentSetKey = "CurrentSet";
    public static string currentCustomSetKey = "CurrentCustomSet";
    public static string currentCustomPuzzleNumberKey = "CurrentCustomPuzzleNumber";

    // Use this for initialization
    void Start ()
    {


        //never lock the first puzzle
        if (setNumber == 1 && puzzleNumber == 1)
            checkLocked = false;

        if (checkLocked)
        {
            if (PlayerPrefs.GetInt(GetLevelString()) == 1)
                locked = false;
            else
                LockButton(); 
        }
            
	}

    public void LockButton()
    {
        Renderer r = GetComponent<Renderer>();
        Button b = GetComponent<Button>();

        if (r != null)
            r.material.color = lockTint;

        if (b != null)
        {
            b.image.color = lockTint;

        }

        locked = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public string GetLevelString()
    {
        return GetLevelString(puzzleNumber);
    }

    public string GetLevelString(int lvlNo)
    {
        if (!customMode)
            return puzzlePrefix + delimiter + setNumber.ToString() + delimiter + lvlNo.ToString();
        else
            return customSetName + customDelimiter + lvlNo.ToString();
    }

    public string GetPrevLevelString()
    {
        return GetLevelString(puzzleNumber-1);
    }

    public void LoadLevel()
    {
        PlayerPrefs.SetInt("RetryCount", 0);

        if (!locked)
        {
            if (!customMode)
                SceneManager.LoadScene(GetLevelString());
            else
            {
                PlayerPrefs.SetString(PuzzleLoader.currentCustomSetKey,customSetName);
                PlayerPrefs.SetInt(PuzzleLoader.currentCustomPuzzleNumberKey, puzzleNumber);
                SceneManager.LoadScene(customLevelLoader);
            }
        }
    }
}
