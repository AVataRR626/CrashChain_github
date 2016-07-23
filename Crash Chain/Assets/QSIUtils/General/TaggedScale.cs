using UnityEngine;
using System.Collections;

public class TaggedScale : MonoBehaviour
{
    public string targetTag = "Player";
    public float scaleFactor = 1.5f;
    public bool destroyAfterEffect = true;

	// Use this for initialization
	void Start ()
    {
        Scale();

        if (destroyAfterEffect)
            Destroy(gameObject);
	}

    public void Scale()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        foreach(GameObject t in targets)
        {
            t.transform.localScale *= scaleFactor;
        }
    }
	

}
