using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]

public class SpriteBlink : MonoBehaviour 
{
	public Color col1;
	public Color col2;
	public float col1BlinkRate;
    public float col2BlinkRate;
    public float delay = 0;

	private SpriteRenderer sr;
	private bool blinkFlag;
	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();
		sr.color = col1;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (delay > 0)
            delay -= Time.deltaTime;
        else
            Blink();
	}

    void Blink()
    {
        if (sr.color == col1 || sr.color == col2)
            blinkFlag = !blinkFlag;

        if (blinkFlag)
        {
            sr.color = Color.Lerp(sr.color, col1, col1BlinkRate);
        }
        else
        {
            sr.color = Color.Lerp(sr.color, col2, col2BlinkRate);
        }
    }
}
