using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GridSpawner))]
public class CrashChainArcadeManager : MonoBehaviour
{
    public int level = 0;

    public static CrashChainArcadeManager instance;

	// Use this for initialization
	void Start ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LevelUp()
    {
        level++;
    }
}
