using UnityEngine;
using System.Collections;

public class TimerBonus : MonoBehaviour 
{

    public bool bonusOnStart = true;
	public float timerBonus = 10;

    QSITimer t;

    // Use this for initialization
    void Start () 
	{
		t = FindObjectOfType(typeof(QSITimer)) as QSITimer;

        if (bonusOnStart)
            AddBonus();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    void AddBonus()
    {
        if (t != null)
        {
            t.timer += timerBonus;
        }

    }
}
