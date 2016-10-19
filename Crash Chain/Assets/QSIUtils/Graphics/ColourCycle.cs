using UnityEngine;
using System.Collections;

public class ColourCycle : MonoBehaviour
{
    public Color[] colourList;
    public float[] displayInterval;
    public float[] displayTime;
    public string[] cycleMessages;

    public SpriteRenderer spr;

    public float startDelay = 0;
    public float displayClock = 0;
    public int displayIndex = 0;
    public float transitionClock = 0;

	// Use this for initialization
	void Start ()
    {
        displayClock = displayTime[0];
        spr = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (startDelay > 0)
            startDelay -= Time.deltaTime;
        else
        {
            ManageColourCycle();
        }
	}

    void ManageColourCycle()
    {
        if (displayIndex >= colourList.Length)
            displayIndex = 0;

        int nextIndex = displayIndex + 1;

        if (nextIndex >= colourList.Length)
            nextIndex = 0;

        Color currentColour = colourList[displayIndex];
        Color nextColour = colourList[nextIndex];

        if (displayClock > 0)
        {
            displayClock -= Time.deltaTime;
            spr.color = currentColour;
        }
        else
        {
            if (transitionClock > 0)
            {
                //float lerpVal = (transitionClock / displayInterval[displayIndex]);
                //spr.color = Color.Lerp(currentColour, nextColour, lerpVal);
                transitionClock -= Time.deltaTime;
            }
            else
            {
                displayIndex = nextIndex;
                displayClock = displayTime[displayIndex];
                transitionClock = displayInterval[displayIndex];
                gameObject.SendMessage(cycleMessages[displayIndex], SendMessageOptions.DontRequireReceiver);

            }

        }
    }
}
