using UnityEngine;
using System.Collections;

public class RaycastMouseFollow : MonoBehaviour 
{
	public bool dragFollow = false;
	public LayerMask affectedMasks;

	void Update() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, affectedMasks))
		{	Debug.DrawLine(ray.origin, hit.point);

			if(! dragFollow)
				transform.position = hit.point;
			else if (Input.GetMouseButton(0))
				transform.position = hit.point;
		}
		
	}
}
