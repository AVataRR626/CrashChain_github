using UnityEngine;
using System.Collections;

public class QSICharConnSpeedBoost : MonoBehaviour
{
    public float speedFactor = 1.5f;
    public bool destroyAfterEffect = true;

    // Use this for initialization
    void Start ()
    {
        QSICharConn[] chars = FindObjectsOfType<QSICharConn>();

        foreach(QSICharConn c in chars)
        {
            c.moveSpeed *= speedFactor;
        }

        if (destroyAfterEffect)
            Destroy(gameObject);
	}

}
