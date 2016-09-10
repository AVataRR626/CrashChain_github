using UnityEngine;
using System.Collections;

public class KeyIntValueMod : MonoBehaviour
{
    public string keyString;
    public int mod;

    private int val;

	// Use this for initialization
	void Start ()
    {
        SetMod();
    }

    public void SetMod()
    {
        val = PlayerPrefs.GetInt(keyString, 0);

        val += mod;

        PlayerPrefs.SetInt(keyString, val);
    }
}
