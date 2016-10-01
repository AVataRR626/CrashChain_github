//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MoveCam))]
public class MoveCamHeroFinder : MonoBehaviour 
{
	public string heroTag = "Hero";
	public GameObject hero;

	// Use this for initialization
	void Start () 
	{
		if(hero == null)
			hero = GameObject.FindGameObjectWithTag(heroTag);

		if(hero != null)
		{	GetComponent<MoveCam>().anchor = hero.transform;
			transform.position = hero.transform.position;
		}
	}
}
