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

	void Start()
	{
		rigidSphere.velocity = forwardSpeed * Vector3.forward;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Unpause()
	{
		Debug.Log("Unpause");
		isPaused = false;
	}

	public override void Pause()
	{

	}

	protected override void PausedUpdate()
	{

	}

	protected override void UnpausedUpdate()
	{

	}
}
