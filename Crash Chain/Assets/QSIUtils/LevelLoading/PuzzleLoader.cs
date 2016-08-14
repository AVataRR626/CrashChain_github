using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleLoader : MonoBehaviour
{
    public string puzzlePrefix = "puzzle_";
    public string delimiter = "_";
    public int setNumber = 1;
    public int puzzleNumber = 1;

    public bool checkLocked = true;
    public bool locked = false;
    public Color lockTint;

	// Use this for initialization
	void Start ()
    {
        //never lock the first puzzle
        if (setNumber == 1 && puzzleNumber == 1)
            checkLocked = false;

        if (checkLocked)
        {
            if (PlayerPrefs.GetInt(GetPrevLevelString()) == 1 || 
                PlayerPrefs.GetInt(GetLevelString()) == 1)
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
        return puzzlePrefix + delimiter + setNumber.ToString() + delimiter + puzzleNumber.ToString();
    }

    public string GetPrevLevelString()
    {
        return puzzlePrefix + delimiter + setNumber.ToString() + delimiter + (puzzleNumber-1).ToString();
    }

    public void LoadLevel()
    {
        if(!locked)
            SceneManager.LoadScene(GetLevelString());
    }
}
