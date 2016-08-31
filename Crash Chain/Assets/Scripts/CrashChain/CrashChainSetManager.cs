using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;

public class CrashChainSetManager : MonoBehaviour
{
    public GameObject setButtonTemplate;
    public static string SetListKey = "SetList";
    public static char SetDelimiter = ';';

    public string[] setList;

    // Use this for initialization
    void Start ()
    {
        setList = GetSets();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public static string [] GetSets()
    {
        string setListString = PlayerPrefs.GetString(SetListKey);
        char[] delim = { ';' };
        return setListString.Split(delim);
    }

    public void NewSetButton()
    {
        int endNum = setList.Length;
        string newSetName = "NewSet_"+endNum;

        AddSet(newSetName);
        PlayerPrefs.SetString(PuzzleLoader.currentCustomSetKey, newSetName);
        PlayerPrefs.SetInt(PuzzleLoader.currentCustomPuzzleNumberKey, 1);
    }

    /*
    Duplicate a given set...
    */
    public void CopySet(string sourceSet, string newSetName)
    {
        AddSet(newSetName);

        for (int i = 0; i < 12; i++)
        {
            string levelString = PlayerPrefs.GetString("lvl:" + sourceSet + ":" + i.ToString());
            PlayerPrefs.SetString("lvl:" + newSetName + ":" + i.ToString(), levelString);

        }
    }
    
    //checks if a set already in the set list
    public static bool SetExists(string set)
    {
        string setListString = PlayerPrefs.GetString(SetListKey);
        return setListString.Contains(set);
    }

    //adds set to the setlist
    public static void AddSet(string set)
    {
        if(!SetExists(set))
        {
            string setListString = PlayerPrefs.GetString(SetListKey);

            if(CountSets(setListString) > 0)
                PlayerPrefs.SetString(CrashChainSetManager.SetListKey, setListString + ";" + set);
            else
                PlayerPrefs.SetString(CrashChainSetManager.SetListKey, set);

        }
    }

    public static int CountSets(string set)
    {
        int count = 0;
        string setListString = PlayerPrefs.GetString(SetListKey);
        count = Regex.Match(setListString, ";").Length;
        Debug.Log("Set Counts: " + count);
        return count;
    }

    //delete a set from setlist
    public static void DeleteSet(string set)
    {

    }
}
