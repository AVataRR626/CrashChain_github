using UnityEngine;
using System.Collections;

//class that handles dynamic loading and reloading
public class CrashChainDynLevelLoader : MonoBehaviour
{
    public bool loadCustomMode = false;
    public string customLevel = "";
    public bool retryMode = true;
    public string serialisedLevel;
    public CrashLink squareLinkPrefab;
    public CrashLink triLinkPrefab;
    public CrashLink hexLinkPrefab;
    public float nextLevelDelay = 1;

    int retryCount = 0;

    string setName;
    int lvlNumber;
    // Use this for initialization
    void Start ()
    {
        if(loadCustomMode)
        {
            if(customLevel == "")
            {
                setName = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetKey);
                lvlNumber = PlayerPrefs.GetInt(PuzzleLoader.currentCustomPuzzleNumberKey) - 1;//index starts from 0, but puzzle numbers startt from 1
                customLevel = setName + ":" + lvlNumber.ToString();
            }

            LoadLevel();
        }

        if(retryMode)
        { 
            if (!PlayerPrefs.HasKey("RetryCount"))
            {
                PlayerPrefs.SetInt("RetryCount", 0);
            }
            else
            {
                HandleRetry();
            }
        }

    }

    [ContextMenu("LoadLevel")]
    public void LoadLevel()
    {
        Debug.Log("LOAD LEVEL");
        CrashChainUtil.ClearLevel();

        if (!loadCustomMode)
            CrashChainUtil.DeserialiseLevel(serialisedLevel, transform, squareLinkPrefab, triLinkPrefab, hexLinkPrefab);
        else
            HandleCustomLoad();
    }

    void HandleCustomLoad()
    {
        
        string levelString = "lvl:" + customLevel;
        Debug.Log("Handling Custom Load: " + levelString);
        serialisedLevel = PlayerPrefs.GetString(levelString);        
        CrashChainUtil.DeserialiseLevel(serialisedLevel, transform, squareLinkPrefab, triLinkPrefab, hexLinkPrefab);
    }

    void HandleRetry()
    {
        retryCount = PlayerPrefs.GetInt("RetryCount");

        if (retryCount > 0)
        {
            //this means the player needs to retry, 
            //try to load the load last state before defeat
            if (PlayerPrefs.HasKey("GameLastLevel"))
            {
                int lastMoves = PlayerPrefs.GetInt("LastMoveCount", 0);
                OverchargeMonitor.instance.AddMoves(lastMoves);
                CrashChainUtil.ClearLevel();
                serialisedLevel = PlayerPrefs.GetString("GameLastLevel");
                CrashChainUtil.DeserialiseLevel(serialisedLevel, transform, squareLinkPrefab, triLinkPrefab, hexLinkPrefab);

            }

        }
    }

    void NextLevel()
    {
        Debug.Log("DynLevelLoader:NextLevel");
        lvlNumber++;
        PlayerPrefs.SetInt(PuzzleLoader.currentCustomPuzzleNumberKey, lvlNumber + 1);
        customLevel = setName + ":" + lvlNumber.ToString();
        //HandleCustomLoad();
    }

    void NextLevelDelayed()
    {
        Invoke("NextLevel", nextLevelDelay);
    }
}
