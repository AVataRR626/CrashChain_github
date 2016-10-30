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

    private Text txt;
    private CrashChainArcadeManager mgr;

	// Use this for initialization
	void Start ()
    {
        Init();
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
        InGameCurrency.AddValue(amount);

        if (CrashChainMonetisationManager.instance != null)
            CrashChainMonetisationManager.instance.shardsEarned += amount;
    }
}
