using UnityEngine;
using System.Collections;

public class AudioMuter : MonoBehaviour
{

    public static string muteKeyString = "MuteMusic";
    bool mute = false;

	// Use this for initialization
	void Start ()
    {
        if(PlayerPrefs.HasKey(muteKeyString))
        {
            int result = PlayerPrefs.GetInt(muteKeyString, 0);
            mute = (result != 0);
        }

        if(mute)
            MuteAll(mute);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void ToggleMute()
    {
        SetMute(!mute);
    }

    public void SetMute(bool m)
    {
        mute = m;
        int prefsVal = 1;

        if (!mute)
            prefsVal = 0;

        PlayerPrefs.SetInt(muteKeyString, prefsVal);
        MuteAll(mute);
    }

    public void MuteAll(bool m)
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach(AudioSource a in audioSources)
        {
            if (m)
                a.Stop();
            else
                a.Play();
        }
    }


}
