using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class CrashChainDisplayUtil : MonoBehaviour
{
    public enum DisplayMode {currentScore,bestScore,levelName};

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
    }
}
