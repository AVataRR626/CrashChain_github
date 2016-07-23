using UnityEngine;
using System.Collections;

//Save Regos to a csv file!
//
//by
//1) Matt Cabanag
//2) ..
//3) ..

public class RegoFileSaver : MonoBehaviour 
{
	public TextEdit3D name;
	public TextEdit3D email;
	public GameObject notification;
	
	public string regoFile = "regos.csv";
	
	// Use this for initialization
	void Start () 
	{
		notification.active = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(name.GetTextInput() != "" && email.GetTextInput() != "")
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				WriteRegistration(name.GetTextInput(),email.GetTextInput());
				
				if(notification != null)
				{
					notification.active = true;
				}
			}
		}
	}
	
	void WriteRegistration(string name, string email)
	{
		#if UNITY_METRO
		//whatever the proper thing is.
		#else
		System.IO.StreamWriter file =
			new System.IO.StreamWriter(regoFile, true);
		
		string newLine = System.DateTime.Now + ", '" + name.Replace("\r","") + "', " + email.Replace("\r","") + "\r\n";
		file.WriteLine(newLine);
		file.Close();
		#endif
		
	}
}
