using UnityEngine;
using System.Collections;

public class CrashChainDynLevelSaver : MonoBehaviour
{
    public string serialisedLevel;
    int retryCount;

	// Use this for initialization
	void Start ()
    {
        if(PlayerPrefs.HasKey("RetryCount"))
        {
            retryCount = PlayerPrefs.GetInt("RetryCount");
        }


	}

    public void ReryIncrement()
    {

        //increment the retry count
        if (!PlayerPrefs.HasKey("RetryCount"))
        {
            PlayerPrefs.SetInt("RetryCount", 1);
        }
        else
        {
            retryCount = PlayerPrefs.GetInt("RetryCount");
            retryCount++;
            PlayerPrefs.SetInt("RetryCount", retryCount);
        }
        
    }

    public void SaveLevel()
    {
        //save the last level config so it can be reloaded later...
        serialisedLevel = CrashChainUtil.SerialiseLevel();
        PlayerPrefs.SetString("GameLastLevel", serialisedLevel);
    }

    public void RegisterWin()
    {
        //reset retry count if you win..
        PlayerPrefs.SetInt("RetryCount", 0);
    }
}
