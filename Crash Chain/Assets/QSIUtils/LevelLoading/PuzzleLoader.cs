using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuzzleLoader : MonoBehaviour
{
    public string puzzlePrefix = "puzzle_";
    public string delimiter = "_";
    public int setNumber = 1;
    public int puzzleNumber = 1;


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadLevel()
    {
        Application.LoadLevel(puzzlePrefix + delimiter + setNumber.ToString() + delimiter + puzzleNumber.ToString());
    }
}
