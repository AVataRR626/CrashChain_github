using UnityEngine;
using System.Collections;

//class that handles dynamic loading and reloading
public class CrashChainDynLevelLoader : MonoBehaviour
{
    public bool loadCustomMode = false;
    public string customLevel;
    public bool retryMode = true;
    public string serialisedLevel;
    public CrashLink squareLinkPrefab;
    public CrashLink triLinkPrefab;
    public CrashLink hexLinkPrefab;

    int retryCount = 0;

    // Use this for initialization
    void Start ()
    {
        if(loadCustomMode)
        {
            HandleCustomLoad();
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

        CrashChainUtil.DeserialiseLevel(serialisedLevel, transform, squareLinkPrefab, triLinkPrefab, hexLinkPrefab);
    }

    void HandleCustomLoad()
    {
        string levelString = "lvl:" + customLevel;
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
                CrashChainUtil.ClearLevel();
                serialisedLevel = PlayerPrefs.GetString("GameLastLevel");
                CrashChainUtil.DeserialiseLevel(serialisedLevel, transform, squareLinkPrefab, triLinkPrefab, hexLinkPrefab);

            }

        }
    }
}
