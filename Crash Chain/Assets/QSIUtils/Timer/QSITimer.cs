using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//TimerClass
//Designed for the survival levels in Protcol E. Probably could
//be used for other games as well.
//
//authors:
//1) Matt Cabanag
//2)
//3)

public class QSITimer : MonoBehaviour 
{
	public bool timerActive = true;
	public bool countdownMode = false;
	public int timerCharCount = 5;
    public float maxTime = 100;
	
	public float timer = 0.1f;
	
	public GameObject [] deathWatch;//stops the clock whenever any one in this list dies
	public GameObject [] wakeWatch;//stops the clock whenever any one in this is awake
	
	
	public GameObject [] zeroClockDeathList;//the list of things to kill when the clock hits zero;
	public GameObject [] zeroClockActivateList;//the list of things to activate when the clock hits zero;
	
	private int countdownFactor = 1;
	TextMesh myTextMesh;
	Text myText;
	
	// Use this for initialization
	void Start () 
	{
		myTextMesh = GetComponent<TextMesh>();
		myText = GetComponent<Text>();
	}
	
	
	void FixedUpdate () 
	{
	
		if((myTextMesh != null || myText != null) && timerActive)
		{
			try
			{
				if(countdownMode)
				{
					countdownFactor = -1;	
				}
				else
				{
					countdownFactor = 1;
				}
				
				
				if(timer > 0)
				{
                    if (timer > maxTime)
                        timer = maxTime;

					timer += Time.fixedDeltaTime * countdownFactor;
				}				
				else
				{
					if(countdownMode)
					{
						timer = 0.00f;
						timerActive = false;
					}
					
					//kill everything in the death list when your each 0;
					foreach(GameObject g in zeroClockDeathList)
					{
						Destroy(g);
					}

					//activate everything in the activate list when you reach 0
					foreach(GameObject g in zeroClockActivateList)
					{
						g.SetActive(true);
					}
				}


                //keep this in the try catch block in case we run out of digits...
                if (myTextMesh != null)
                    myTextMesh.text = timer.ToString().Substring(0, timerCharCount);

                if (myText != null)
                    myText.text = timer.ToString().Substring(0, timerCharCount);

            }
			catch
			{
                if (myTextMesh != null)
                    myTextMesh.text = "0";

                if (myText != null)
                    myText.text = "0";
            }
		}



        if(deathWatch != null)
            for (int i = 0; i < deathWatch.Length; i++)
		    {
			    if(deathWatch[i] == null)
			    {
				    timerActive = false;
				    i = deathWatch.Length;
			    }
		    }
		
        if(wakeWatch != null)
		    for(int i = 0; i < wakeWatch.Length; i++)
		    {
			    if(wakeWatch[i] != null)
			    {
				    if(wakeWatch[i].activeSelf)
				    {
					    timerActive = false;
					    i = wakeWatch.Length;
				    }
			    }
		    }
    }

    public void Pause()
    {
        timerActive = false;
    }
	
	public void AddTimer(float newTime)
	{
		timer += newTime;
	}
}
