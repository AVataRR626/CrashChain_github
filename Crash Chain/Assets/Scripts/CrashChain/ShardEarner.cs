using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShardEarner : MonoBehaviour
{
    public int amount = 1;
    public string prefix = "shards earned: ";
    public bool syncAmountToSet = true;
    public bool addOnStart = true;
    public float addOnStartDelay = 0.5f;

    private Text txt;

	// Use this for initialization
	void Start ()
    {
        txt = GetComponent<Text>();

        if (syncAmountToSet)
        {
            amount = LevelNavigator.instance.setNumber;
        }

        if (txt != null)
        {
            txt.text = prefix + amount.ToString();
        }
    }

    void OnEnable()
    {

        if (addOnStart)
        {
            Invoke("Add", addOnStartDelay);
        }

        CrashChainMonetisationManager.instance.Init();
    }

    public void Add()
    {
        InGameCurrency.AddValue(amount);

        if (CrashChainMonetisationManager.instance != null)
            CrashChainMonetisationManager.instance.shardsEarned += amount;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
