//General QSI Utility
//
//Music Manager.cs
//-
//
//by 
//1) Matt Cabanag
//2) ...
//3) ...

using UnityEngine;
using System.Collections;

public class VolumeManager : MonoBehaviour 
{
	public bool persistent = false;
	public float mainVolume = 0.5f;
	public float musicVolume = 0.5f;
	public float spinSyncDelta = 0.01f;

	private MusicManager mscMgr;
	private float timeStamp;
	private static VolumeManager first;

	void OnLevelWasLoaded(int level)
	{

		if(first == null)
			first = this;

		Init();
	}

	// Use this for initialization
	void Start () 
	{
		Init();
	}

	void Init()
	{
		mscMgr = FindObjectOfType<MusicManager>();
		
		VolumeManager [] mgrList = FindObjectsOfType<VolumeManager>();
		
		//make sure you're the only one.
		if(mgrList.Length == 1)
		{
			if(persistent)
			{
				DontDestroyOnLoad(gameObject);
				
				foreach(Transform child in transform)
				{
					DontDestroyOnLoad(child.gameObject);
				}
			}
		}
		else
		{
			if(this != first)
				Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		musicVolume = Mathf.Clamp (musicVolume,0,1);
		mainVolume = Mathf.Clamp(mainVolume,0,1);

		if(mscMgr != null)
			mscMgr.GetComponent<AudioSource>().volume = musicVolume;

		AudioListener.volume = mainVolume;
	}

	void SpinSyncMain(int delta)
	{
		//Debug.Log ("SpinSyncMain:"+delta);

		if(delta > 0)
			mainVolume += spinSyncDelta;
		else if (delta < 0)
			mainVolume -= spinSyncDelta;
	}

	void SpinSyncMusic(int delta)
	{
		if(delta > 0)
			musicVolume += spinSyncDelta;
		else if (delta < 0)
			musicVolume -= spinSyncDelta;
	}
}
