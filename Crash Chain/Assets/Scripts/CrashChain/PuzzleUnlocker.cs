using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuzzleUnlocker : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        string levelName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(levelName, 1);
	}
}
