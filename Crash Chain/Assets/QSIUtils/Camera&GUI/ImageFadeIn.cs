using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFadeIn : MonoBehaviour
{
    public Image img;
    public Text txt;
    // Use this for initialization
    public float fadeInRate = 1;
    public float startDelay = 0;

    private float [] originalAlpha = new float[2];

	void Start ()
    {
        img = GetComponent<Image>();
        txt = GetComponent<Text>();

        if (img != null)
        {
            originalAlpha[0] = img.color.a;
            img.color = SetAlpha(img.color, 0);
        }

        if (txt != null)
        {
            originalAlpha[1] = txt.color.a;
            txt.color = SetAlpha(txt.color, 0);
        }
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (startDelay <= 0)
            FadeIn();
        else
            startDelay -= Time.deltaTime;
    }

    void FadeIn()
    {
        if (img != null)
        {
            img.color = FadeIn(img.color, originalAlpha[0], fadeInRate);
        }

        if (txt != null)
        {
            txt.color = FadeIn(txt.color, originalAlpha[1], fadeInRate);
        }
    }

    public static Color FadeIn(Color c, float goalAlpha, float increment)
    {
        if(c.a < goalAlpha)
        {
            c.a += increment * Time.deltaTime;
        }

        return c;
    }

    public static Color SetAlpha(Color c, float newAlpha)
    {
        c.a = newAlpha;
        return c;
    }
}
