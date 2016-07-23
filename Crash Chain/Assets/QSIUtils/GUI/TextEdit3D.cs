using UnityEngine;
using System.Collections;

public class TextEdit3D : MonoBehaviour 
{
	public string initialStringValue = "default.lvl";
	public string highlightChar = ">";
	public string cursorChar = "|";
	public float cursorFlashInterval = 0.5f;

	private TextMesh textMesh;
	private string textInput;
	private string originalText;
	private bool highlighted = false;
	private float cursorCountdown;
	private string cursor = "";
	
	// Use this for initialization
	void Start () 
	{
		textMesh = GetComponent<TextMesh>();
		originalText = textMesh.text;
		cursorCountdown = cursorFlashInterval;
		textInput = initialStringValue;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(highlighted)
		{
			//textInput += Input.inputString;
			
			foreach (char c in Input.inputString) 
			{
	            if (c == "\b"[0])
				{
					if (textInput.Length != 0)
					{
						textInput = textInput.Substring(0,textInput.Length - 1);
					}
				}    
	            else
				{
	                textInput += c;
				}
			}
			
			if(cursorCountdown > 0)
			{
				cursorCountdown -= Time.deltaTime;
			}
			else
			{
				cursorCountdown = cursorFlashInterval;
				if(cursor == cursorChar)
				{
					cursor = "";
				}
				else
				{
					cursor = cursorChar;
				}
			}
			
			
			textMesh.text = "> " + originalText + "  " + textInput + cursor;
			
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
			{
				UnHighlight();
			}
		}
		else
		{
			textMesh.text = originalText + "  " + textInput;
		}
	}
	
	void OnMouseDown()
	{
		//transform.root.gameObject.BroadcastMessage("UnHighlight");
		Highlight();
	}
	
	void Highlight()
	{
		transform.root.gameObject.BroadcastMessage("UnHighlight");
		highlighted = true;
	}
	
	void UnHighlight()
	{
		highlighted = false;
	}
	
	public string GetTextInput()
	{
		return textInput;
	}

}
