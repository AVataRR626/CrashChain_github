using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class QSIKillableHealthTxt : MonoBehaviour 
{
	public QSIKillable subject;

	private TextMesh qacText;

	// Use this for initialization
	void Start () 
	{


		qacText = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(subject != null)
		{
			qacText.text = subject.health.ToString ();
		}
	}
}
