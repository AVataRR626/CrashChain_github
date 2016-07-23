using UnityEngine;
using System.Collections;

public class QSIGlobalHealthBoost : MonoBehaviour
{
    public float healthBonus;


	// Use this for initialization
	void Start ()
    {
        QSIKillable []  killables = FindObjectsOfType<QSIKillable>();

        foreach (QSIKillable k in killables)
            k.AddHealth(healthBonus);

	}
	
}
