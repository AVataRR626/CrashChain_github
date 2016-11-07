using UnityEngine;
using System.Collections;

public class Slowdown : MonoBehaviour
{
    public float goalTimescale = 0.5f;
    public float slowdownRate = 0.01f;

	// Use this for initialization
	void Start ()
    {
        
	}

    void OnDisable()
    {
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.timeScale > goalTimescale)
        {
            Time.timeScale -= slowdownRate * Time.deltaTime;
        }
        else
        {
            Time.timeScale = goalTimescale;
        }
	}
}
