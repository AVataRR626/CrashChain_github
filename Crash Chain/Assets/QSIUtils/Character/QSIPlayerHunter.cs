using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class QSIPlayerHunter : MonoBehaviour
{
    public float damageDistance = 1;
    public float damage = 0.25f;
    public string targetTag = "Player";
    public GameObject target;

    public NavMeshAgent navAgent;

	// Use this for initialization
	void Start ()
    {
        navAgent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            target = GenUtils.FindClosestWithTag(transform.position, targetTag);
            navAgent.destination = target.transform.position;

        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (target != null)
        {
            navAgent.destination = target.transform.position;

            if(Vector3.Distance(target.transform.position,transform.position) <= damageDistance)
            {
                QSIKillable k = target.GetComponent<QSIKillable>();
                k.Damage(damage);
            }
        }
        else
            target = GenUtils.FindClosestWithTag(transform.position, targetTag);
    }
}
