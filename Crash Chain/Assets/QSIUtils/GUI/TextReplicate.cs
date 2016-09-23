using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class TextReplicate : MonoBehaviour
{
    public Text reference;
    public bool continuousReplicate = true;

    Text txt;

	// Use this for initialization
	void Start ()
    {
        txt = GetComponent<Text>();

        txt.text = reference.text;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (continuousReplicate)
            txt.text = reference.text;
	}
}
