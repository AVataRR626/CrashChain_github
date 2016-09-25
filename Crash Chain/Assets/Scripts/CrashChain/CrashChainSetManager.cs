using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;

public class CrashChainSetManager : MonoBehaviour
{
    public GameObject setButtonTemplate;
    public static string SetListKey = "SetList";
    public static char LevelDelimiter = (char)512;

    public PuzzleMenuGenerator myGenerator;
    public string[] setList;

    // Use this for initialization
    void Start ()
    {
        setList = GetSets();
        myGenerator = FindObjectOfType<PuzzleMenuGenerator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ImportButton()
    {
        string newSetName = "ImportedSet_" + System.DateTime.Now.ToString("yymmddHHmmss");

        AddSetButton(newSetName);
    }

    public void NewSetButton()
    { 
        string newSetName = "NewSet_" + System.DateTime.Now.ToString("yymmddHHmmss");

        AddSetButton(newSetName);
    }

    public void AddSetButton(string newSetName)
    {
        int endNum = setList.Length + 1;

        AddSet(newSetName);
        PlayerPrefs.SetString(PuzzleLoader.currentCustomSetNameKey, newSetName);
        PlayerPrefs.SetInt(PuzzleLoader.currentCustomSetNumberKey, endNum);
        PlayerPrefs.SetInt(PuzzleLoader.currentCustomPuzzleNumberKey, 1);
    }

    public void DeleteCurrentSet()
    {
        if(myGenerator != null)
        {
            Debug.Log("Delete this: " + setList[myGenerator.setNumber - 1]);
            DeleteSet(setList[myGenerator.setNumber-1]);
        }
    }

    public static string [] GetSets()
    {
        string setListString = PlayerPrefs.GetString(SetListKey);
        char[] delim = { ';' };
        return setListString.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
    }

    /*
    Duplicate a given set...
    */
    public static void CopySet(string sourceSet, string newSetName)
    {
        AddSet(newSetName);

        for (int i = 0; i < 12; i++)
        {
            string levelString = PlayerPrefs.GetString("lvl:" + sourceSet + ":" + i.ToString());
            PlayerPrefs.SetString("lvl:" + newSetName + ":" + i.ToString(), levelString);

        }
    }

    public static void RenameSet(string oldName, string newName)
    {
        string setList = PlayerPrefs.GetString(SetListKey);
        setList = setList.Replace(oldName, newName);
        PlayerPrefs.SetString(SetListKey, setList);

        for (int i = 0; i < 12; i++)
        {
            //copy the old levels...
            string levelString = PlayerPrefs.GetString("lvl:" + oldName + ":" + i.ToString());
            PlayerPrefs.SetString("lvl:" + oldName + ":" + i.ToString(), levelString);
            PlayerPrefs.DeleteKey("lvl:" + oldName + ":" + i.ToString());

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

            if (CountSets(setListString) > 0)
                PlayerPrefs.SetString(SetListKey, setListString + ";" + set );
            else
                PlayerPrefs.SetString(SetListKey, set);

        }
    }

    public static int CountSets(string set)
    {
        string[] setNames = GetSets();
        return setNames.Length;
    }

    //delete a set from setlist
    public static void DeleteSet(string set)
    {
        string setListString = PlayerPrefs.GetString(SetListKey);

        
        if(setListString != set)
        { 
            //if you aren't the last set...
            if (setListString.Contains(set + ";"))
            { 
                //general case:
                //get rid of the set from the set list...
                setListString = setListString.Replace(set + ";", "");
                PlayerPrefs.SetString(SetListKey, setListString);
            }
            else if(setListString.Contains(";" + set))
            {
                //if this set appears at the end of the setList
                setListString = setListString.Replace(";"+ set,"");
                PlayerPrefs.SetString(SetListKey, setListString);
            }
        }
        else
        {
            //and clear the list if this is the only set..
            PlayerPrefs.SetString(SetListKey, "");
        }



        for (int i = 0; i < 12; i++)
        {
            PlayerPrefs.DeleteKey("lvl:" + set + ":" + i.ToString());

        }
    }

    public static string GetSetString(string set)
    {
        string result = "";

        for (int i = 0; i < 12; i++)
        {
            //get set data from internal storage...
            result += PlayerPrefs.GetString("lvl:" + set + ":" + i.ToString());

            //Debug.Log("lvl:" + set + ":" + i.ToString() + "; " + PlayerPrefs.GetString("lvl:" + set + ":" + i.ToString()).Length);

            //separate each set with a delimiter
            if(i <11)
                result += CrashChainSetManager.LevelDelimiter;

        }

        return result;
    }

    public static bool ValidSetString(string setString)
    {
        string[] newLevels = setString.Split(LevelDelimiter);

        return newLevels.Length == 12;
    }

    public static void ImportSet(string setString, string setName)
    {
        string[] newLevels = setString.Split(LevelDelimiter);

        if(newLevels.Length != 12)
        {
            Debug.LogError("ImportSet: Trying to import invalid set! Level count should be 12. Is: " + newLevels.Length);
        }
        else
        {
            if(!SetExists(setName))
            {
                AddSet(setName);
            }

            for(int i = 0; i < 12; i++)
            {
                PlayerPrefs.SetString("lvl:" + setName + ":" + i.ToString(), newLevels[i]);
            }
        }
    }
}
