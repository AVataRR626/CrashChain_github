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
	
	public float timer = 0.1f;
	
	public GameObject [] deathWatch;//stops the clock whenever any one in this list dies
	public GameObject [] wakeWatch;//stops the clock whenever any one in this is awake
	
	
	public GameObject [] zeroClockDeathList;//the list of things to kill when the clock hits zero;
	public GameObject [] zeroClockActiveateList;//the list of things to activate when the clock hits zero;
	
	private int countdownFactor = 1;
	TextMesh myTextMesh;
	Text myText;
	
	// Use this for initialization
	void Start () 
	{
		myTextMesh = GetComponent<TextMesh>();
		myText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
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
					timer += Time.deltaTime * countdownFactor;					
				}				
				else
				{
					if(countdownMode)
					{
						timer = 0;
						timerActive = false;
					}
					
					//kill everything in the death list when your each 0;
					foreach(GameObject g in zeroClockDeathList)
					{
						Destroy(g);
					}

					//activate everything in the activate list when you reach 0
					foreach(GameObject g in zeroClockActiveateList)
					{
						g.SetActive(true);
					}
				}
				

				timer += Time.deltaTime * countdownFactor;

				if(myTextMesh != null)
					myTextMesh.text = timer.ToString().Substring(0,timerCharCount);

				if(myText != null)
					myText.text = timer.ToString().Substring(0,timerCharCount);
			}
			catch
			{
				
			}
		}
		
		for(int i = 0; i < deathWatch.Length; i++)
		{
			if(deathWatch[i] == null)
			{
				timerActive = false;
				i = deathWatch.Length;
			}
		}
		
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

	
	public void AddTimer(float newTime)
	{
		timer += newTime;
	}
}
