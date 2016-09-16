using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lzf; 

public class CrashChainImportExportManager : MonoBehaviour
{
    public InputField exportText;
    public string currentCustomSet;

	// Use this for initialization
	void Start ()
    {
        currentCustomSet = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetExportText()
    {
        currentCustomSet = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);

        exportText.text = CrashChainSetManager.GetSetString(currentCustomSet);
    }
}
