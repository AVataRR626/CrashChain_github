using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TypeMaster : MonoBehaviour
{
    public static TypeMaster Instance;

    //specifies the type colours...
    //type/colour pair is by index...
    //i.e. index 0, is type 0; registered colour is corresponding colour.
    public Color [] typeColours;

    //specifies the type-pair triggers..
    //i.e. if index 0 has 0, it means type 0 triggers type 0.
    //i.e. if index 0 has type 1, it means type 0 triggers type 1.
    public int [] typeMatch;

    public Color outlineColour;
    public Color immovableColour = new Color(101,101,101,255);

    // Use this for initialization
    void Start ()
    {
	    if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
	}

    public void SetTypeColour0(string hexString)
    {
        SetTypeColour(0, hexString);
    }

    public void SetTypeColour1(string hexString)
    {
        SetTypeColour(1, hexString);
    }

    public void SetTypeColour2(string hexString)
    {
        SetTypeColour(2, hexString);
    }

    public void SetTypeColour(int type, string hexString)
    {
        Debug.Log("TypeMaster: SetTypeColour:" + type.ToString() + " : " + hexString);

        if (!hexString.Contains("#"))
            hexString = "#" + hexString;

        ColorUtility.TryParseHtmlString(hexString, out typeColours[type]);
    }

    public void SetOutlineColour(string hexString)
    {
        Debug.Log("TypeMaster: SetOutlineColour:" + hexString);

        if (!hexString.Contains("#"))
            hexString = "#" + hexString;
        ColorUtility.TryParseHtmlString(hexString, out outlineColour);

        ColourOutlines();
    }

    public void ColourOutlines()
    {
        CrashLink[] crashLinks = FindObjectsOfType<CrashLink>();

        foreach (CrashLink c in crashLinks)
            c.ColourOutlines(c.transform);
    }
    
    public void SetBackgroundColour(string hexString)
    {
        Debug.Log("TypeMaster: SetBackgroundColour:" + hexString);

        if (!hexString.Contains("#"))
            hexString = "#" + hexString;

        Color newCol;
        ColorUtility.TryParseHtmlString(hexString, out newCol);

        Camera.main.backgroundColor = newCol;
    }
}
