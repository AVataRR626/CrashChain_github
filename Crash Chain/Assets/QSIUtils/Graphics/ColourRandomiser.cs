using UnityEngine;
using System.Collections;

public class ColourRandomiser : MonoBehaviour
{
    public Color maxAdd;

    private SpriteRenderer spr;

	// Use this for initialization
	void Start ()
    {
        spr = GetComponent<SpriteRenderer>();

        Randomise();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Randomise()
    {

        Color colourAdd = new Color();
        colourAdd.r = Random.Range(0, maxAdd.r);
        colourAdd.g = Random.Range(0, maxAdd.g);
        colourAdd.b = Random.Range(0, maxAdd.b);
        colourAdd.a = Random.Range(0, maxAdd.a);

        if (spr != null)
        {
            spr.color += colourAdd;
        }
    }
}

