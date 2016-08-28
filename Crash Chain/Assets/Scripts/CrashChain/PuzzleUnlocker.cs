using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuzzleUnlocker : MonoBehaviour
{
    public bool customMode = false;
    public string levelName;
    public int bestMoves = -1;
    LevelNavigator myNavigator;


    public static PuzzleUnlocker instance;

    // Use this for initialization
    void Start ()
    {
        instance = this;

        myNavigator = FindObjectOfType<LevelNavigator>();

        if (!customMode)
        { 
            levelName = SceneManager.GetActiveScene().name;            
        }
        else if(myNavigator != null)
        {
            levelName = myNavigator.GetSceneName();
        }

        //check for any stored "best score"; unlock the level by regoing one..
        if (PlayerPrefs.GetInt(levelName, -1) == -1)
        {
            bestMoves = -1;
            PlayerPrefs.SetInt(levelName, -1);
        }
        else
            bestMoves = PlayerPrefs.GetInt(levelName);
    }
}
