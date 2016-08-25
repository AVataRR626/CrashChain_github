using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuzzleUnlocker : MonoBehaviour
{
    public bool customMode = false;

    LevelNavigator myNavigator;
    string levelName;

    // Use this for initialization
    void Start ()
    {
        myNavigator = FindObjectOfType<LevelNavigator>();

        if (!customMode)
        { 
            levelName = SceneManager.GetActiveScene().name;            
        }
        else if(myNavigator != null)
        {
            levelName = myNavigator.GetSceneName();
        }

        PlayerPrefs.SetInt(levelName, 1);
    }
}
