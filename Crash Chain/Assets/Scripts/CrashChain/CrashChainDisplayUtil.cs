using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class CrashChainDisplayUtil : MonoBehaviour
{
    public enum DisplayMode {currentScore,bestScore,levelName,overchargeCount,overchargeLeft,arcadeLevel,puzzleButtonLevel,puzzleButtonBestScore,movesLeft,currentCustomSet,shardCount,slotStatus,slotCount,shardsEarned};

    public string prefix = "";
    public DisplayMode displayMode;
    Text txt;

	// Use this for initialization
	void Start ()
    {
        txt = GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        txt.text = prefix;

        if (displayMode == DisplayMode.currentScore)
            txt.text += OverchargeMonitor.instance.GetMoves().ToString();

        if (displayMode == DisplayMode.bestScore)
        {
            if (PuzzleUnlocker.instance.bestMoves > -1)
                txt.text += PuzzleUnlocker.instance.bestMoves.ToString();
            else
                txt.text = "";
        }

        if (displayMode == DisplayMode.levelName)
            txt.text += PuzzleUnlocker.instance.levelName;

        if (displayMode == DisplayMode.overchargeCount)
            txt.text = CrashLink.overchargeCount.ToString();

        if (displayMode == DisplayMode.overchargeLeft)
            txt.text = OverchargeMonitor.instance.RemainingOvercharges().ToString();

        if (displayMode == DisplayMode.arcadeLevel)
            txt.text = CrashChainArcadeManager.instance.level.ToString();

        if (displayMode == DisplayMode.puzzleButtonLevel)
        {

            PuzzleLoader myPl = GetPuzzleLoader();

            if(myPl != null)
                txt.text = myPl.puzzleNumber.ToString();
        }

        if (displayMode == DisplayMode.puzzleButtonBestScore)
        {

            PuzzleLoader myPl = GetPuzzleLoader();

            if (myPl != null)
            { 
                string levelName = myPl.GetLevelString();

                int bestScore = PlayerPrefs.GetInt(levelName, -1);

                if(bestScore >= 0)
                {
                    txt.text = bestScore.ToString();
                }
            }
        }

        if(displayMode == DisplayMode.movesLeft)
        {
            txt.text = MovesMonitor.instance.GetMovesLeft().ToString();
        }

        if(displayMode == DisplayMode.currentCustomSet)
        {
            txt.text = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);
        }

        if(displayMode == DisplayMode.shardCount)
        {
            txt.text = InGameCurrency.GetCurrentValue().ToString();
        }

        if(displayMode == DisplayMode.slotStatus)
        {
            txt.text = CrashChainMonetisationManager.instance.GetSlotStringI();
        }

        if(displayMode == DisplayMode.slotCount)
        {
            txt.text = CrashChainMonetisationManager.GetSlotCount().ToString();
        }

        if(displayMode == DisplayMode.shardsEarned)
        {
            txt.text = CrashChainMonetisationManager.instance.shardsEarned.ToString();
        }
    }

    //get the first PuzzleLoader you find going up your tree...
    PuzzleLoader GetPuzzleLoader()
    {
        PuzzleLoader myPl = GetComponent<PuzzleLoader>();
        Transform t = transform;

        while(myPl == null && t.parent != null)
        {
            myPl = t.GetComponent<PuzzleLoader>();

            if (myPl != null)
                return myPl;

            t = t.parent;
        }

        return null;
    }
}
