using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger_Car : Danger
{
	Rigidbody rigid;
	public Vector3 centerOfMass;
	public float forwardSpeed = 5.0f;
	// Start is called before the first frame update
	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		rigid.centerOfMass = centerOfMass;
	}

	protected override void StartDanger()
	{
		rigid.velocity = forwardSpeed * transform.forward;
	}

	private void OnDrawGizmosSelected()
	{
		if (rigid == null)
		{
			rigid = GetComponent<Rigidbody>();
			rigid.centerOfMass = centerOfMass;
		}
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(rigid.worldCenterOfMass, 0.2f);
	}
}
