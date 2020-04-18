using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger_Car : Danger
{
	Rigidbody rigidSphere;
	public float forwardSpeed = 5.0f;
	// Start is called before the first frame update
	void Awake()
	{
		rigidSphere = GetComponent<Rigidbody>();
	}

	protected override void StartDanger()
	{
		rigidSphere.velocity = forwardSpeed * transform.forward;
	}
}
