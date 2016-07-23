using UnityEngine;
using System.Collections;

public class BumpMessage : MonoBehaviour 
{
	public GameObject [] targetList;
	public string message;
	public string stringParam;
	public float floatParam;
	public bool messageBumped = true;
	public string tagFilter = "";

	void OnControllerColliderHit(ControllerColliderHit coll)
	{
		//Debug.Log ("BUMP MESSAGE, OnControllerColliderHit");
		
		DeliverMessaes(coll.gameObject);
	}

	void OnCollisionEnter(Collision coll)
	{
		//Debug.Log ("BUMP MESSAGE, OnCollisionEnter");

		foreach(GameObject o in targetList)
		{
			o.SendMessage(message,stringParam,SendMessageOptions.DontRequireReceiver);
		}

		if(messageBumped)
			coll.gameObject.SendMessage(message,stringParam,SendMessageOptions.DontRequireReceiver);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//Debug.Log ("BUMP MESSAGE, OnCollisionEnter2D");

		foreach(GameObject o in targetList)
		{
			o.SendMessage(message,stringParam,SendMessageOptions.DontRequireReceiver);
		}

		if(messageBumped)
			coll.gameObject.SendMessage(message,stringParam,SendMessageOptions.DontRequireReceiver);
	}

	public void DeliverMessaes(GameObject hitObject)
	{
		DeliverMessages(hitObject, ref targetList, message, stringParam, floatParam, tagFilter, messageBumped);
	}

	public static void DeliverMessages(GameObject hitObject, ref GameObject [] tgtList, string m, string sp, float fp, string tgFltr, bool msgBmp)
	{
		foreach(GameObject o in tgtList)
		{
			if(o.tag == tgFltr || tgFltr == "")
			{
				Debug.Log (" DELIVERING MESSAGE!!");

				if(sp != null)
					o.SendMessage(m,sp,SendMessageOptions.DontRequireReceiver);

				if(fp != null)
					o.SendMessage(m,fp,SendMessageOptions.DontRequireReceiver);
			}
		}
		
		if(msgBmp)
		{
			if(hitObject.tag == tgFltr || tgFltr == "")
			{
				Debug.Log (" DELIVERING MESSAGE!!");

				if(sp != null)
					hitObject.SendMessage(m,sp,SendMessageOptions.DontRequireReceiver);
				
				if(fp != null)
					hitObject.SendMessage(m,fp,SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
