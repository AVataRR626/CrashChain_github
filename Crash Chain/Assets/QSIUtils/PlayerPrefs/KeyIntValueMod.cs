using UnityEngine;
using System.Collections;

public class KeyIntValueMod : MonoBehaviour
{
    public string keyString;
    public int mod;
    public bool hardSet = false;
    public bool deleteKey = false;

    private int val;

	// Use this for initialization
	void Start ()
    {
        if (!hardSet)
            SetMod();
        else
            HardSet();
            
    }

    public void DeleteKey()
    {
        PlayerPrefs.DeleteKey(keyString);
    }

    public void SetMod()
    {
        val = PlayerPrefs.GetInt(keyString, 0);

        val += mod;

        PlayerPrefs.SetInt(keyString, val);
    }

    public void HardSet()
    {

        PlayerPrefs.SetInt(keyString, mod);
    }
}
