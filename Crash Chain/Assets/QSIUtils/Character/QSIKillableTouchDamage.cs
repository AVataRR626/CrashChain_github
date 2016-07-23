using UnityEngine;
using System.Collections;

[RequireComponent(typeof(QSIKillable))]
public class QSIKillableTouchDamage : MonoBehaviour
{
    public string damageTag = "Lava";
    public float damageRate = 0.5f;

    private QSIKillable myKillable;

	// Use this for initialization
	void Start ()
    {
        myKillable = GetComponent<QSIKillable>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnControllerColliderHit(ControllerColliderHit coll)
    {
        if(coll.gameObject.tag == damageTag)
        {
            myKillable.Damage(damageRate);
        }
    }
}
