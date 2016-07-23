using UnityEngine;
using System.Collections;

public class SpawnGrenade2D : MonoBehaviour
{
    public bool spawnOnStart = true;
    public bool killOnSpawn = true;
    public Rigidbody2D [] spawnObjects;
    public Vector3[] spawnForce;
    public float forceFactor = 10;

	// Use this for initialization
	void Start ()
    {
        if (spawnOnStart)
            Spawn();

        if (killOnSpawn)
            Destroy(gameObject);
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        if(spawnObjects.Length != spawnForce.Length)
        {
            Debug.LogError("spawn objects and spawn forces do not match");
            return;
        }

        int i = 0;
        
        foreach(Rigidbody2D r in spawnObjects)
        {
            Rigidbody2D rb = Instantiate(r, transform.position, Quaternion.identity) as Rigidbody2D;
            rb.AddForce(spawnForce[i] * forceFactor);

            i++;
        }
    }
}
