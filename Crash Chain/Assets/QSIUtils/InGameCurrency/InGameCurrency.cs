/*
 * QSI in game currency management untility
 * 
 * 30 Oct 2016
 * 
 * Matt Cabanag
 *
 */

using UnityEngine;
using System.Collections;

public class InGameCurrency : MonoBehaviour
{
    public static string IGCKey = "Shards";

    public int GetCurrentValueI()
    {
        return PlayerPrefs.GetInt(IGCKey, 0);
    }

    public void ModValueI(int amt)
    {
        AddValue(amt);
    }

    public bool PurchaseI(int amt)
    {
        return Purchase(amt);
    }

    public static int GetCurrentValue()
    {
        //make sure the key exists...
        if (!PlayerPrefs.HasKey(IGCKey))
        {
            PlayerPrefs.SetInt(IGCKey, 0);
        }

        return PlayerPrefs.GetInt(IGCKey);
    }

    //includes both deposit and withdrawal...
    public static void AddValue(int amt)
    {
        //make sure the key exists...
        if(!PlayerPrefs.HasKey(IGCKey))
        {
            PlayerPrefs.SetInt(IGCKey, 0);
        }

        int balance = PlayerPrefs.GetInt(IGCKey);
        balance += amt;

        PlayerPrefs.SetInt(IGCKey, balance);
    }

    public static bool Purchase(int amt)
    {
        int balance = GetCurrentValue();

        if(balance - amt >= 0)
        {
            AddValue(-amt);
            return true;
        }

        return false;
    }
}
