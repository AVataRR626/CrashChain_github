using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture bg;
	public Texture start;
	public int imgW = 960;
	public int imgH = 600;
	public string levelName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
			Application.LoadLevel(levelName);
		}
	}

	void OnGUI()
	{
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), bg);
		GUI.DrawTexture (new Rect ((Screen.width / 2) - (imgW /2), (Screen.height / 2) - (imgH /2), imgW, imgH), start);
	}
}
