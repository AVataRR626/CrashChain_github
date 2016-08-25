using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelNavigator : MonoBehaviour
{
    
    public string puzzlePrefix = "puzzle_";
    public char delimiter = '_';
    public int setNumber = -1;
    public int puzzleNumber = -1;
    public bool autoSetBySceneName = true;
    public bool customMode = false;

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
            puzzlePrefix = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetKey);
            int.TryParse(PlayerPrefs.GetString(PuzzleLoader.currentCustomPuzzleNumberKey), out puzzleNumber);
        }
	}
	
	void Update () {
	
	}
}
