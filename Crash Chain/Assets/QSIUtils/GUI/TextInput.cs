using UnityEngine;
using System.Collections;

[ RequireComponent(typeof(GUIText)) ]
public class TextInput : MonoBehaviour 
{
	public string highlightChar = ">";
	public string cursorChar = "|";
	public float cursorFlashInterval = 0.5f;
	
	private string textInput;
	private string originalText;
	private bool highlighted = false;
	private float cursorCountdown;
	private string cursor = "";

	// Use this for initialization
	void Start () 
	{
		originalText = GetComponent<GUIText>().text;
		cursorCountdown = cursorFlashInterval;
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
			
			
			GetComponent<GUIText>().text = "> " + originalText + "  " + textInput + cursor;
		}
		else
		{
			GetComponent<GUIText>().text = originalText + "  " + textInput;
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
