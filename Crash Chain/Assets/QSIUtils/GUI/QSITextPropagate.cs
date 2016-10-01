using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
BounceBlaster Project
TextMeshPropagateText.cs
Syncs up the text in a TextMesh tree

Fakes the zoom-in/out effect for an orthographic camera.

Authors:
1) Matt Cabanag
2) ..
3) ..

*/


public class QSITextPropagate : MonoBehaviour 
{
	public bool autoSync = true;
	
	private TextMesh qacTextMesh;
	private Text qacText;

	private string actualText;
	private int fontSize = 1;

	// Use this for initialization
	void Start () 
	{
		qacTextMesh = GetComponent<TextMesh>();
		qacText = GetComponent<Text>();


		SyncText();
		
	}
	
	void OnDrawGizmosSelected()
	{
		SyncText();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(autoSync)
		{
			if(qacText != null)
			{	actualText = qacText.text;
				fontSize = qacText.fontSize;
			}

			if(qacTextMesh != null)
			{	actualText = qacTextMesh.text;
				fontSize = qacTextMesh.fontSize;
			}

			SyncText();
		}
	}
	
	public void SyncText()
	{
		SyncText(actualText, fontSize, transform);
	}
	
	void SyncText(string actualText, int fontSize, Transform t)
	{
		foreach(Transform child in t)
		{
			TextMesh txtMsh = child.GetComponent<TextMesh>();
			if(txtMsh != null)
			{
				txtMsh.text = actualText;
				txtMsh.fontSize = fontSize;
			}

			Text txt = child.GetComponent<Text>();
			if(txt != null)
			{
				txt.text = actualText;
				txt.fontSize = fontSize;
			}

			SyncText(actualText, fontSize, child);
		}

	}
}
