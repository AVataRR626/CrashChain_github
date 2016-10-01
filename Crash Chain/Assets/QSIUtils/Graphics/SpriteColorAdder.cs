using UnityEngine;
using System.Collections;

public class SpriteColorAdder : MonoBehaviour
{
    public SpriteRenderer spr;    
    public Color colourAdd = new Color(0.2f, 0.2f, 0.2f);
    public bool addSwitch = false;

    private Color baseColour;

    // Use this for initialization
    void Start ()
    {
        spr = GetComponent<SpriteRenderer>();
        baseColour = spr.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Color col = baseColour;

        if (addSwitch)
            col += colourAdd;

        spr.color = col;
    }

    void AddColour()
    {
        addSwitch = true;
    }

    void BaseColour()
    {
        addSwitch = false;
    }
}
