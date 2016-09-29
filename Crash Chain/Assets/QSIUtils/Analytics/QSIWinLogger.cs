using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class QSIWinLogger : MonoBehaviour
{

    public bool logOnStart = true;

	// Use this for initialization
	void Start ()
    {
        if (logOnStart)
            AnalyticsLog();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AnalyticsLog()
    {
        
        string sceneName = SceneManager.GetActiveScene().name;

        Analytics.CustomEvent("win", new Dictionary<string, object>
            {
                { "level",sceneName}
            }
        );

        Debug.Log("Anayitics: " + sceneName);

    }
}
