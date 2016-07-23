using UnityEngine;
using System.Collections;

public class QSIDamageArming : MonoBehaviour 
{

	public QSIDamager myDamager;

	// Use this for initialization
	void Start () {
		myDamager = GetComponentInChildren<QSIDamager>();
	}
	
	void ArmDamage(bool arm)
	{
		if(myDamager != null)
			myDamager.ArmDamage(arm);
	}

	void ArmToggle(int arm)
	{
		if(arm > 0)
			BroadcastMessage ("ArmDamage",true);
		else
			BroadcastMessage ("ArmDamage",false);
	}
}
