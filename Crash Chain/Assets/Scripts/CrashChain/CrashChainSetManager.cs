using UnityEngine;
using System.Collections;

public class CrashChainSetManager : MonoBehaviour
{
    public GameObject setButtonTemplate;
    public static string SetListKey = "SetList";

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

    //adds set tot he setlist
    public static void AddSet(string set)
    {
        if(!SetExists(set))
        {
            string setListString = PlayerPrefs.GetString(SetListKey);
            PlayerPrefs.SetString(CrashChainSetManager.SetListKey, setListString + ";" + set);

        }
    }

    //delete a set from setlist
    public static void DeleteSet(string set)
    {

    }
}
