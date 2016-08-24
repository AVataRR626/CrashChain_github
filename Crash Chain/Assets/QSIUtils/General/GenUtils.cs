//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class GenUtils : MonoBehaviour {

	public static T [] CullNulls<T>(ref T[] big)
	{

		T [] rawCopy = DeepCopy<T>(ref big);
		
		int nullIndex = 0;

		int end = rawCopy.Length;
		for(int i = 0; i < end; i++)
		{
			if(rawCopy[i] == null)
			{

				for(int j = i; j < end - 1; j++)
				{
					rawCopy[j] = rawCopy[j + 1];
				}
				i--;
				end--;
			}
		}
		
		T [] notNullSmall = new T[end];
		
		for(int i = 0; i < end; i++)
		{
			notNullSmall [i] = rawCopy[i];
		}

		//Debug.Log("CullNulls:"+end);
		
		return notNullSmall;
	}
	
	public static T [] DeepCopy<T>(ref T[] orig)
	{
		//Debug.Log ("GenUtils:Doing Deep Copy");

		T [] copy = new T[orig.Length];

		int i;
		for(i = 0; i < orig.Length; i++)
		{
			copy[i] = orig[i];
		}
		//Debug.Log ("GenUtils:DeepCopy:"+i);
		
		return copy;
	}

	
	//quickly stolen and modified from:
	//http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
	public static void Shuffle<T>(ref T[] list)  
	{  
		int n = list.Length;  
		while (n > 1) {  
			n--;  
			int k = Random.Range (0,n); 
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public static void ShuffleRectTransforms(ref RectTransform[] list)  
	{  
		int n = list.Length;  
		while (n > 1) 
		{  
			n--;  
			int k = Random.Range (0,n); 

			Vector3 value = list[k].position;  
			list[k].position = list[n].position;  
			list[n].position = value;  
		}  
	}


	public static T [] MergeList<T>(ref T[] listA,ref T[] listB)
	{
		T [] result = new T[listA.Length+listB.Length];

		int ri = 0;

		for(int i = 0; i < listA.Length; i++)
		{
			result[ri] = listA[i];
			ri++;
		}

		for(int i = 0; i < listB.Length; i++)
		{
			result[ri] = listB[i];
			ri++;
		}

		return result;
	}

	public static void SetActiveObjects(ref GameObject [] list, bool mode)
	{
		foreach(GameObject o in list)
		{
			if(o != null)
				o.SetActive(mode);
		}
	}

	public static int FindClosestGameObjectIndex(Vector3 refPos, ref GameObject [] list)
	{
		float closest = 10000;
		
		int result = -1;
		
		
		for (int i = 0; i < list.Length; i++) 
		{
			GameObject u = list [i];
			if (u != null) 
			{
				float distance = Vector3.Distance (u.transform.position, refPos);
				if (distance < closest) 
				{
					closest = distance;
					result = i;
				}
			}
		}
		
		
		return result;
	}

	public static int FindClosestGameObjectIndex(Vector3 refPos, ref GameObject [] list, float maxRange)
	{
		float closest = 10000;
		
		int result = -1;
		
		
		for (int i = 0; i < list.Length; i++) 
		{
			GameObject u = list [i];
			if (u != null) 
			{
				float distance = Vector3.Distance (u.transform.position, refPos);
				if (distance < closest && distance <= maxRange) 
				{
					closest = distance;
					result = i;
				}


			}
		}
		
		
		return result;
	}

	public static GameObject FindClosestWithTag(Vector3 refPos, string tag)
	{
		float closest = 10000;
		
		GameObject result = null;
		GameObject [] list = GameObject.FindGameObjectsWithTag(tag);

		return FindClosestGameObject(refPos,ref list);

		/*
		foreach(GameObject u in list)
		{
			if(u != null)
			{
				float distance = Vector3.Distance(u.transform.position, refPos);
				
				if(distance < closest)
				{	closest = distance;
					result = u;
				}
			}
		}
		
		
		return result;
		*/
	}

	public static GameObject FindClosestGameObject(Vector3 refPos, ref GameObject [] list)
	{
		float closest = 10000;
		
		GameObject result = null;
		
		
		foreach(GameObject u in list)
		{
			if(u != null)
			{
				float distance = Vector3.Distance(u.transform.position, refPos);
				
				if(distance < closest)
				{	closest = distance;
					result = u;
				}
			}
		}
		
		
		return result;
	}

	public static GameObject FindClosestGameObject(Vector3 refPos, ref GameObject [] list, float maxRange)
	{
		float closest = 10000;
		
		GameObject result = null;
		
		
		foreach(GameObject u in list)
		{
			if(u != null)
			{
				float distance = Vector3.Distance(u.transform.position, refPos);
				
				if(distance < closest && distance <= maxRange)
				{	closest = distance;
					result = u;
				}
			}
		}
		
		
		return result;
	}

	public static T GetFirstAncestorOfType<T>(Transform t) where T: UnityEngine.Component
	{
		T searchItem = t.GetComponent<T>();

		if(t.parent != null)
		{
			//Debug.Log("GETFirstAncestorOfType:"+t.name);

			if(searchItem != null)
			{
				//Debug.Log("GETFirstAncestorOfType:FOUND!!");
				return searchItem;
			}
			else
			{
				//Debug.Log("GETFirstAncestorOfType:NotFound");
				return GetFirstAncestorOfType<T>(t.parent);
			}
		}

		//Debug.Log ("GETFirstAncestorOfType:ReachedTop:"+t.name);

		//if(searchItem == null)
			//Debug.Log ("GETFirstAncestorOfType:TopNotFound");

		return searchItem;
	}

	//"There can only be one"
	public static void Highlander<T>(T o) where T: UnityEngine.MonoBehaviour
	{
		T[] rivals = GameObject.FindObjectsOfType<T>();

		foreach(T c in rivals)
		{
			if(c != o)
				Destroy (c.gameObject);
		}
	}

	//"There can only be one"
	public static void Highlander<T>(T o, float delay) where T: UnityEngine.MonoBehaviour
	{
		T[] rivals = GameObject.FindObjectsOfType<T>();
		
		foreach(T c in rivals)
		{
			if(c != o)
				Destroy (c.gameObject,delay);
		}
	}

	public static void Shout(ref GameObject [] list, string message)
	{
		foreach(GameObject target in list)
			if(target != null)
				target.SendMessage(message);
	}

	public static void Shout(ref GameObject [] list, string message, int intParam)
	{
		foreach(GameObject target in list)
			if(target != null)
				target.SendMessage(message,intParam);
	}

	public static float Min(float a, float b)
	{
		if(a < b)
		{
			return a;
		}

		return b;
	}

    public static void SpawnList(GameObject [] list, Transform spawnPoint)
    {
        Debug.Log("SpawnList Begin:");
        foreach(GameObject o in list)
        {
            Debug.Log("SpawnList:" + o.name);
            Instantiate(o, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
