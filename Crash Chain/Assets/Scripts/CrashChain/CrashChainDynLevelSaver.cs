using UnityEngine;
using System.Collections;

public class CrashChainDynLevelSaver : MonoBehaviour
{
    public string serialisedLevel;
    int retryCount;

    public bool customMode = false;

    LevelNavigator myNavigator;
    string levelName;

    // Use this for initialization
    void Start ()
    {
        if(PlayerPrefs.HasKey("RetryCount"))
        {
            retryCount = PlayerPrefs.GetInt("RetryCount");
        }

        if(PlayerPrefs.GetInt("LastMoveCount",-1)==-1)
            PlayerPrefs.SetInt("LastMoveCount", OverchargeMonitor.instance.GetMoves());
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
        PlayerPrefs.SetInt("LastMoveCount", OverchargeMonitor.instance.GetMoves());
    }

    public void ResetRetry()
    {
        //reset retry count if you win..
        PlayerPrefs.SetInt("RetryCount", 0);
        PlayerPrefs.SetInt("LastMoveCount", 0);
    }

    public void RegisterWin()
    {
        ResetRetry();

        //also save the best score in player prefs
        string lName = PuzzleUnlocker.instance.levelName;
        int bestScore = PlayerPrefs.GetInt(lName, 100);
        int score = OverchargeMonitor.instance.GetMoves();

        if (bestScore <= -1)// > -1 means player has completed this level for the first time
        {
            PlayerPrefs.SetInt(lName, score);
            PuzzleUnlocker.instance.bestMoves = score;
        }
        else if (score < bestScore)//less moves is better...
        {
            PlayerPrefs.SetInt(lName, score);
            PuzzleUnlocker.instance.bestMoves = score;
        }

        //Debug.Log("-----------saving score:" + score + "," + bestScore + "," + lName);
    }
}
