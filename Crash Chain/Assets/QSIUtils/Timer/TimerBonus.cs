using UnityEngine;
using System.Collections;

public class TimerBonus : MonoBehaviour 
{
	
	public float timerBonus = 10;
	
	// Use this for initialization
	void Start () 
	{
		QSITimer t = FindObjectOfType(typeof(QSITimer)) as QSITimer;
		
		if(t != null)
		{
			t.timer += timerBonus;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
