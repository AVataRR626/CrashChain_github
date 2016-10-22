using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFadeIn : MonoBehaviour
{
    public Image img;
    public Text txt;
    public TextMesh txtmsh;
    public SpriteRenderer spr;
    // Use this for initialization
    public float fadeInRate = 1;
    public float startDelay = 0;
    public bool fadeOutMode = false;

    private float [] originalAlpha = new float[4];
    private float startAlpha = 0;

    private float delayClock;
    private bool startedSwitch = false;
    private bool finishedSwitch = false;
    
    public float DelayClock
    {
        get
        {
            return delayClock;
        }
    }

    public bool Started
    {
        get
        {
            return startedSwitch;
        }
    }

    public bool Finished
    {
        get
        {
            return finishedSwitch;
        }
    }

	void Start ()
    {
        Init();
    }

    public void Init()
    {
        img = GetComponent<Image>();
        txt = GetComponent<Text>();
        txtmsh = GetComponent<TextMesh>();
        spr = GetComponent<SpriteRenderer>();

        Reset();
    }

    public void Reset()
    {
        delayClock = startDelay;

        if (fadeOutMode)
            startAlpha = 1;

        if (img != null)
        {
            originalAlpha[0] = img.color.a;
            img.color = SetAlpha(img.color, startAlpha);
        }

        if (txt != null)
        {
            originalAlpha[1] = txt.color.a;
            txt.color = SetAlpha(txt.color, startAlpha);
        }

        if (txtmsh != null)
        {
            originalAlpha[2] = txtmsh.color.a;
            txtmsh.color = SetAlpha(txtmsh.color, startAlpha);
        }

        if(spr != null)
        {
            originalAlpha[3] = spr.color.a;
            spr.color = SetAlpha(spr.color, startAlpha);
        }

        startedSwitch = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (delayClock <= 0)
        {
            if (fadeOutMode)
                FadeOut();
            else
                FadeIn();

            if (!startedSwitch)
                startedSwitch = true;

            if (!finishedSwitch)
                if (ReachedOriginalAlpha())
                    finishedSwitch = true;
        }
        else
        {
            delayClock -= Time.deltaTime;
        }
    }

    void FadeOut()
    {
        //Debug.Log("Fade Out:");
        if (img != null)
        {
            img.color = FadeOut(img.color, 0, fadeInRate);
        }

        if (txt != null)
        {
            txt.color = FadeOut(txt.color, 0, fadeInRate);
        }

        if(txtmsh != null)
        {
            txtmsh.color = FadeOut(txtmsh.color, 0, fadeInRate);
        }

        if(spr != null)
        {
            spr.color = FadeOut(spr.color, 0, fadeInRate);
        }
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

        if (txtmsh != null)
        {
            txtmsh.color = FadeIn(txtmsh.color, originalAlpha[2], fadeInRate);
        }

        if (spr != null)
        {
            spr.color = FadeIn(spr.color, originalAlpha[3], fadeInRate);
        }
    }

    public bool ReachedOriginalAlpha()
    {
        if (img != null)
        {
            return (img.color.a >= originalAlpha[0]);
        }

        if (txt != null)
        {
            //txt.color = FadeIn(txt.color, originalAlpha[1], fadeInRate);

            return (txt.color.a >= originalAlpha[1]);
        }

        if (txtmsh != null)
        {
            //txtmsh.color = FadeIn(txtmsh.color, originalAlpha[2], fadeInRate);

            return (txtmsh.color.a >= originalAlpha[2]);
        }

        if (spr != null)
        {
            //spr.color = FadeIn(spr.color, originalAlpha[3], fadeInRate);

            return (spr.color.a >= originalAlpha[3]);
        }

        return false;
    }

    public static Color FadeOut(Color c, float goalAlpha, float increment)
    {
        //Debug.Log("Fade Out:" + c.a);
        if (c.a > goalAlpha)
        {
            //Debug.Log("Yup");
            c.a -= increment * Time.deltaTime;
        }

        return c;
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
