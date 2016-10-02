/*
QSI General Utils
PerlinShake - used for anything that needs perlin shaking, including screen shakes.

-Matt Cabanag
2 Oct 2016
*/


using UnityEngine;
using System.Collections;

public class PerlinShake : MonoBehaviour
{
    public float shakeTime = 0.2f;
    public bool shakeSwitch = false;
    public float turbulence = 3;
    public float shakeScale = 0.75f;

    private float clock = 0;
    Vector3 anchorPos;
    

	// Use this for initialization
	void Start ()
    {
        anchorPos = transform.position;
        clock = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(clock > 0)
        { 
	        if(shakeSwitch)
            {
                anchorPos = transform.position;
                shakeSwitch = false;
            }

            Vector2 noise = new Vector2();
            //noise.x = 0Mathf.PerlinNoise(clock * turbulence, -clock * turbulence);
            noise.y = Mathf.PerlinNoise(-clock * turbulence, clock * turbulence);
            noise *= shakeScale;

            transform.position = anchorPos + (Vector3)noise;

            clock -= Time.deltaTime;
        }
    }

    [ContextMenu("Shake")]
    public void StartShake()
    {
        clock = shakeTime;
        shakeSwitch = true;
    }
}
