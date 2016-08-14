using UnityEngine;
using System.Collections;

public class PulseMessage : MonoBehaviour
{
    public GameObject subject;
    public string startMessage = "TouchGlow";
    public string endMessage = "TouchDim";
    public float pulseLength = 0.2f;
    public float pulseInterval = 0.1f;
    public int pulseCount = 2;
    public float resetCLock = 2;

    private float pulseClock;
    private float pulseCountTracker;

	// Use this for initialization
	void Start ()
    {
        Init();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void Init()
    {
        pulseCountTracker = pulseCount;
        PulseStart();
    }

    void PulseStart()
    {
        subject.SendMessage(startMessage);
        Invoke("PulseEnd", pulseLength);
    }

    void PulseEnd()
    {
        pulseCountTracker--;
        subject.SendMessage(endMessage);

        if(pulseCountTracker > 0)
            Invoke("PulseStart", pulseInterval);
        else
        {
            Invoke("Init", resetCLock);
        }
    }
}
