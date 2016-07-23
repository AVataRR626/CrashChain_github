using UnityEngine;
using System.Collections;

public class TextureScrolling : MonoBehaviour 
{
	public bool positionTied = false;
	
	public float xSpeed = 1;
	public float ySpeed = 0;
	
	public bool restrictOffset = false;
	public float maxOffsetX = 0.5f;
	public float minOffsetX = 0.8f;
	public float maxOffsetY = 0;
	public float minOffsetY = 0;
	
	
	private float xOffset = 0;
	private float yOffset = 0;
	
	private Vector3 prevXY;

	// Use this for initialization
	void Start () 
	{
		prevXY = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 xyDelta = (transform.position - prevXY);
		
		if(!positionTied)
		{
			if(xSpeed != 0)
				xOffset += Time.deltaTime * xSpeed;
			
			if(ySpeed != 0)
				yOffset += Time.deltaTime * ySpeed;
		}
		else
		{
			if(xSpeed != 0)
				xOffset += Time.deltaTime * xSpeed * xyDelta.magnitude;
			
			if(ySpeed != 0)
				yOffset += Time.deltaTime * ySpeed * xyDelta.magnitude;
		}
		
		
		
		
		if(restrictOffset)
		{
			if(maxOffsetX > xOffset)
			{
				xOffset = minOffsetX;
			}
			
			if(minOffsetX < xOffset)
			{
				xOffset = maxOffsetX;
			}
			
			if(maxOffsetY > yOffset)
			{
				yOffset = minOffsetY;
			}
			
			if(minOffsetY < yOffset)
			{
				yOffset = maxOffsetY;
			}
		}
		
		if(GetComponent<Renderer>() != null)
		{
			GetComponent<Renderer>().material.mainTextureOffset =  new Vector2(xOffset,yOffset);
		}
		
		prevXY = transform.position;
		
	}
}
