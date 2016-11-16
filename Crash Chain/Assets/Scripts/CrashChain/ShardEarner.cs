using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShardEarner : MonoBehaviour
{
    public int amount = 1;
    public string prefix = "shards earned: ";
    public bool syncAmountToSet = true;
    public bool syncAmountToArcadeLevel = false;
    public bool addOnStart = true;
    public float addOnStartDelay = 0.5f;
    public bool earnOnBest = false;

    private Text txt;
    private CrashChainArcadeManager mgr;

    void Awake()
    {
        //earn shards if you have'nt earned them before...
        if (earnOnBest)
        {
            Debug.Log("WHATS MY MOVE: " + PuzzleUnlocker.instance.bestMoves);

            if (PuzzleUnlocker.instance.bestMoves > -1)
            {
                //only earn shards if you get a better score than previously..
                if (PuzzleUnlocker.instance.bestMoves <= OverchargeMonitor.instance.GetMoves())
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("**NEW SCORE***");
            }
        }
    }

	// Use this for initialization
	void Start ()
    {   

    }

    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        mgr = CrashChainArcadeManager.instance;

        txt = GetComponent<Text>();

        if (syncAmountToSet)
        {
            amount = LevelNavigator.instance.setNumber;
        }

        if (syncAmountToArcadeLevel)
        {
            amount = Mathf.Max(1, mgr.level);
        }

        if (addOnStart)
        {
            Invoke("Add", addOnStartDelay);
        }

        if (txt != null)
        {
            txt.text = prefix + amount.ToString();
        }

        CrashChainMonetisationManager.instance.Init();
    }

    public void Add()
    {
        Debug.Log("ShardEarner: " + name + " : " + amount);
        InGameCurrency.AddValue(amount);

        if (CrashChainMonetisationManager.instance != null)
            CrashChainMonetisationManager.instance.shardsEarned += amount;
    }
}
