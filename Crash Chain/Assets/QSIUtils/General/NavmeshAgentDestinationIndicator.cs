using UnityEngine;
using System.Collections;

public class NavmeshAgentDestinationIndicator : MonoBehaviour 
{
	public NavMeshAgent myAgent;

	// Use this for initialization
	void Start () 
	{
		if(myAgent == null)
			myAgent = transform.root.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = myAgent.destination;
	
	}
}
