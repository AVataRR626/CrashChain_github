using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrashChainMonetisationManager : MonoBehaviour
{
    public static CrashChainMonetisationManager instance;
    public static string PuzzleSlotKey = "PuzzleSlots";
    public static string UnlimitedKey = "Unlimited";

    [Header("Primary Stats")]
    public int shards = 0;
    public int setCount = 0;
    public int slotCount = 0;

    [Header("Set Count Management")]
    public GameObject[] overSetLimitDisable;
    public GameObject[] overSetLimitEnable;

    [Header("Shards Management")]
    public GameObject[] noShardsDisable;
    public GameObject[] noShardsEnable;

    private CrashChainSetManager setMgr;
    private bool unlimited = false;

	// Use this for initialization
	void Start ()
    {
        Init();
    }

    public void CheckLimits()
    {
        if(!unlimited)
        { 
            if(setCount >= slotCount)
            {
                SetEnabled(overSetLimitEnable, true);
                SetEnabled(overSetLimitDisable, false);
            
            }
            else
            {
                SetEnabled(overSetLimitDisable, true);
                SetEnabled(overSetLimitEnable, false);
            }

            if(shards <= 0)
            {
                Debug.Log("------There are no shards!!");
                SetEnabled(noShardsDisable, false);
                SetEnabled(noShardsEnable, true);
            }
            else
            {
                Debug.Log("+++++There are shards");
                SetEnabled(noShardsDisable, true);
                SetEnabled(noShardsEnable, false);
            }
        }
    }

    public void Init()
    {
        instance = this;

        setMgr = FindObjectOfType<CrashChainSetManager>();

        if(setMgr != null)
            setCount = setMgr.setList.Length;


        if(!PlayerPrefs.HasKey(UnlimitedKey))
        {
            InitLimited();
        }
        else
        {
            InitUnlimited();
        }
    }

    public void InitLimited()
    {
        shards = InGameCurrency.GetCurrentValue();
        slotCount = GetSlotCount();
        InitSlots();

        InitLimited();
        CheckLimits();
        //Invoke("CheckLimits", 0.1f);
    }

    public void InitUnlimited()
    {
        unlimited = true;

        //get all microtransaction objects and disable them
        GameObject[] m = GameObject.FindGameObjectsWithTag("Microtransactions");

        foreach (GameObject o in m)
            o.SetActive(false);

        gameObject.SetActive(false);
    }


    public string GetSlotStringI()
    {
        return setCount.ToString() + "/" + slotCount.ToString() + " slots used";
    }
	
	public void ShardsPurchase(int amt)
    {
        InGameCurrency.Purchase(amt);
        shards = InGameCurrency.GetCurrentValue();
        CheckLimits();
    }

    public void ShardsRefund(int amt)
    {
        InGameCurrency.ModValue(amt);
        shards = InGameCurrency.GetCurrentValue();
        CheckLimits();
    }

    public static void InitSlots()
    {
        if (!PlayerPrefs.HasKey(PuzzleSlotKey))
        {
            PlayerPrefs.SetInt(PuzzleSlotKey, 3);
        }
    }


    public static int GetSlotCount()
    {
        InitSlots();
        return PlayerPrefs.GetInt(PuzzleSlotKey);
    }

    public static void SetEnabled(GameObject[] list, bool mode)
    {
        foreach (GameObject o in list)
        {
            Button b = o.GetComponent<Button>();

            if (b != null)
                b.interactable = mode;
            else
                o.SetActive(mode);
        }
    }
}
